using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Mov4Anyone.Cards;
using Mov4Anyone.CognitiveModels;
using Mov4Anyone.Models;
using Mov4Anyone.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mov4Anyone.Dialogs
{
    public class VideoDialog : ComponentDialog
    {
        private readonly TMDBService _tmdbService;
        public VideoDialog(TMDBService service) : base(nameof(VideoDialog))
        {
            _tmdbService = service;
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt("Prompt", AdaptiveCardVerifier));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                 InitialStepAsync,
                 FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private Task<bool> AdaptiveCardVerifier(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            if (promptContext.Context.Activity.Value != null)
            {

            }
            return Task.FromResult(true);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisResult = (Movies4Anyone)stepContext.Options;
            switch (luisResult.TopIntent().intent)
            {
                case Movies4Anyone.Intent.searchMovie:
                    var lookupJson = await _tmdbService
                        .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.searchMovie.ToString()], luisResult.Entities.movie.FirstOrDefault(), null);

                    var movieResult = JsonConvert.DeserializeObject<SearchResult<MovieResult>>(lookupJson);
                    var opts = new AdaptiveCardGenerator().GenerateMovieSearchAttachment(movieResult);


                    await stepContext.Context.SendActivityAsync(opts.Prompt);
                    opts.Prompt = new Activity(type: ActivityTypes.Typing);
                    return await stepContext.PromptAsync("Prompt", opts, cancellationToken);

                case Movies4Anyone.Intent.searchTV:

                    lookupJson = await _tmdbService
                        .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.searchTV.ToString()], luisResult.Entities.tv_show.FirstOrDefault(), null);

                    var tvResult = JsonConvert.DeserializeObject<SearchResult<TvResult>>(lookupJson);
                    var optsTV = new AdaptiveCardGenerator().GenerateTvSearchAttachment(tvResult);


                    await stepContext.Context.SendActivityAsync(optsTV.Prompt);
                    optsTV.Prompt = new Activity(type: ActivityTypes.Typing);
                    return await stepContext.PromptAsync("Prompt", optsTV, cancellationToken);


                case Movies4Anyone.Intent.searchPeople:

                    lookupJson = await _tmdbService
                        .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.searchPeople.ToString()], luisResult.Entities.person.FirstOrDefault(), null);

                    var peopleResult = JsonConvert.DeserializeObject<SearchResult<PersonResult>>(lookupJson);
                    var optsPerson = new AdaptiveCardGenerator().GeneratePersonSearchAttachment(peopleResult);


                    await stepContext.Context.SendActivityAsync(optsPerson.Prompt);
                    optsPerson.Prompt = new Activity(type: ActivityTypes.Typing);
                    return await stepContext.PromptAsync("Prompt", optsPerson, cancellationToken);
            }
            
            return await stepContext.NextAsync(0, cancellationToken);
        }


        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choiceResult = JsonConvert.DeserializeObject<PersonSearchModel>(stepContext.Context.Activity.Value.ToString());

            PromptOptions opts = new PromptOptions();
            switch (choiceResult.Type)
            {
                case "Movie":
                    var info = await _tmdbService
                        .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.movieDetails.ToString()], "", choiceResult.Id);
                    var movieDetails = JsonConvert.DeserializeObject<MovieDetails>(info);
                    opts = new AdaptiveCardGenerator().GenerateMovieDetailsAttachment(movieDetails);
                    break;

                case "Tv":
                    info = await _tmdbService
                        .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.tvDetails.ToString()], "", choiceResult.Id);
                    var tVDetails = JsonConvert.DeserializeObject<TVDetails>(info);
                    opts = new AdaptiveCardGenerator().GenerateTVDetailsAttachment(tVDetails);
                    break;

                case "Person":
                    info = await _tmdbService
                        .FetchInformation(TMDBEndpoints.APIEndpoints["personDetails"], "", choiceResult.Id);
                    var personDetails = JsonConvert.DeserializeObject<PersonDetails>(info);
                    opts = new AdaptiveCardGenerator().GeneratePersonDetailsAttachment(personDetails, choiceResult.KnownFor);
                    break;

            }

            await stepContext.Context.SendActivityAsync(opts.Prompt);
            opts.Prompt = new Activity(type: ActivityTypes.Typing);
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
