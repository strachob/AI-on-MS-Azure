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
    public class SearchDialog : ComponentDialog
    {
        private readonly TMDBService _tmdbService;
        public SearchDialog(TMDBService service) : base(nameof(SearchDialog))
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
            var luisResult = (Movies4Anyone)stepContext?.Options;

            PromptOptions opts = null;
            switch (luisResult.TopIntent().intent)
            {
                case Movies4Anyone.Intent.searchMovie:
                    if (luisResult.Entities.movie != null)
                    {
                        var lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.searchMovie.ToString()], luisResult.Entities.movie.FirstOrDefault(), null);

                        var movieResult = JsonConvert.DeserializeObject<SearchResult<MovieResult>>(lookupJson);
                        opts = new AdaptiveCardGenerator().GenerateMovieSearchAttachment(movieResult);
                    }
                    else
                    {
                        var promptMessage = MessageFactory.Text("I didn't get the name of the movie. Could you repeat please?", "I didn't get the name of the movie. Could you repeat please?", InputHints.ExpectingInput);
                        return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                    }
                    break;

                case Movies4Anyone.Intent.searchTV:
                    if (luisResult.Entities.tv_show != null)
                    {
                        var lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.searchTV.ToString()], luisResult.Entities.tv_show.FirstOrDefault(), null);

                        var tvResult = JsonConvert.DeserializeObject<SearchResult<TvResult>>(lookupJson);
                        opts = new AdaptiveCardGenerator().GenerateTvSearchAttachment(tvResult);
                    }
                    else
                    {
                        var promptMessage = MessageFactory.Text("I didn't get the name of the tv show. Could you repeat please?", "I didn't get the name of the tv show. Could you repeat please?", InputHints.ExpectingInput);
                        return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                    }
                    break;

                case Movies4Anyone.Intent.searchPeople:
                    if (luisResult.Entities.person != null)
                    {
                        var lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.searchPeople.ToString()], luisResult.Entities.person.FirstOrDefault(), null);

                        var peopleResult = JsonConvert.DeserializeObject<SearchResult<PersonResult>>(lookupJson);
                        opts = new AdaptiveCardGenerator().GeneratePersonSearchAttachment(peopleResult);
                    }
                    else
                    {
                        var promptMessage = MessageFactory.Text("I didn't get the name of this person. Could you repeat please?", "I didn't get the name of this person. Could you repeat please?", InputHints.ExpectingInput);
                        return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                    }
                    break;
            }

            if (opts != null)
            {
                await stepContext.Context.SendActivityAsync(opts.Prompt);
                opts.Prompt = new Activity(type: ActivityTypes.Typing);
                return await stepContext.PromptAsync("Prompt", opts, cancellationToken);
            }

            return await stepContext.NextAsync(0, cancellationToken);
        }


        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            PersonSearchModel choiceResult = new PersonSearchModel();
            try
            {
                choiceResult = JsonConvert.DeserializeObject<PersonSearchModel>(stepContext.Context.Activity.Value.ToString());
            }
            catch (NullReferenceException)
            {
                if (stepContext.Result is string text)
                {
                    var luis = stepContext.Options as Movies4Anyone;
                    if (luis.TopIntent().intent == Movies4Anyone.Intent.searchMovie)
                    {
                        ((Movies4Anyone)stepContext.Options).Entities.movie = new string[] { text };
                    }
                    else if(luis.TopIntent().intent == Movies4Anyone.Intent.searchTV)
                    {
                        ((Movies4Anyone)stepContext.Options).Entities.tv_show = new string[] { text };
                    }
                    else
                    {
                        ((Movies4Anyone)stepContext.Options).Entities.person = new string[] { text };
                    }
                    return await stepContext.ReplaceDialogAsync(InitialDialogId, options: stepContext.Options, cancellationToken);
                }
            }

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
