using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Mov4Anyone.Cards;
using Mov4Anyone.CognitiveModels;
using Mov4Anyone.Models;
using Mov4Anyone.Services;
using Newtonsoft.Json;
using NuGet.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mov4Anyone.Dialogs
{
    public class RecommendationDialog : ComponentDialog
    {
        private readonly TMDBService _tmdbService;
        public RecommendationDialog(TMDBService service) : base(nameof(RecommendationDialog))
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
                case Movies4Anyone.Intent.recommendMovie:
                    if (luisResult.Entities.movie != null)
                    {
                        var lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.searchMovie.ToString()], luisResult.Entities.movie.FirstOrDefault(), null);
                        var movieResult = JsonConvert.DeserializeObject<SearchResult<MovieResult>>(lookupJson);
                        var recommendJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.recommendMovie.ToString()], "", movieResult.Results.FirstOrDefault().Id);
                        var recommendationResult = JsonConvert.DeserializeObject<MovieRecommedation>(recommendJson);
                        opts = new AdaptiveCardGenerator().GenerateRecommendedMoviesAttachment(recommendationResult);
                    }
                    else
                    {
                        var promptMessage = MessageFactory.Text("Please tell me the title of movie I can base my recommendation", "Please tell me the title of movie I can base my recommendation", InputHints.ExpectingInput);
                        return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                    }
                    
                    break;
                   
                case Movies4Anyone.Intent.recommendTv:
                    if (luisResult.Entities.tv_show != null)
                    {
                        var lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.searchTV.ToString()], luisResult.Entities.tv_show.FirstOrDefault(), null);
                        var tvResult = JsonConvert.DeserializeObject<SearchResult<TvResult>>(lookupJson);
                        var recommendJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.recommendTv.ToString()], "", tvResult.Results.FirstOrDefault().Id);
                        var recommendationResult = JsonConvert.DeserializeObject<TVRecommendation>(recommendJson);
                        opts = new AdaptiveCardGenerator().GenerateRecommendedTvAttachment(recommendationResult);
                        
                    }
                    else
                    {
                        var promptMessage = MessageFactory.Text("Please tell me the title of a tv show I can base my recommendation", "Please tell me the title of a tv show I can base my recommendation", InputHints.ExpectingInput);
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

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            SearchModel choiceResult = new SearchModel();
            try
            {
                choiceResult = JsonConvert.DeserializeObject<SearchModel>(stepContext.Context.Activity.Value.ToString());
            }
            catch (Exception)
            {
                if (stepContext.Result is string text)
                {
                    var luis = stepContext.Options as Movies4Anyone;
                    if (luis.TopIntent().intent == Movies4Anyone.Intent.recommendMovie)
                    {
                        ((Movies4Anyone)stepContext.Options).Entities.movie = new string[] { text };
                    }
                    else
                    {
                        ((Movies4Anyone)stepContext.Options).Entities.tv_show = new string[] { text };
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
            }
            

            await stepContext.Context.SendActivityAsync(opts.Prompt);
            opts.Prompt = new Activity(type: ActivityTypes.Typing);
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
