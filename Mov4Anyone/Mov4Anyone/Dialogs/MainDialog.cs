
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using Mov4Anyone.Cards;
using Mov4Anyone.CognitiveModels;
using Mov4Anyone.Models;
using Mov4Anyone.Services;
using Newtonsoft.Json;

namespace Mov4Anyone.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly MovieRecognizer _luisRecognizer;
        private readonly TMDBService _tmdbService;
        protected readonly ILogger Logger;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(MovieRecognizer luisRecognizer, ILogger<MainDialog> logger, TMDBService tmdbService)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            _tmdbService = tmdbService;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt("Prompt", AdaptiveCardVerifier));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                ResolveIdStepAsync, 
                FinalStepAsync,
            }));

            AddDialog(new TextPrompt("AskForText"));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private Task<bool> AdaptiveCardVerifier(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            if (promptContext.Context.Activity.Value != null)
            {

            }
            return Task.FromResult(true);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {            
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            var messageText = stepContext.Options?.ToString() ?? "What can I help you with?\nI know a lot about movies and tv shows!\nDon't be afraid to ask :)";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                return await stepContext.BeginDialogAsync(nameof(TextPrompt), cancellationToken);
            }

            var luisResult = await _luisRecognizer.RecognizeAsync<Movies4Anyone>(stepContext.Context, cancellationToken);

            try
            {

                switch (luisResult.TopIntent().intent)
                {

                    case Movies4Anyone.Intent.searchMovie:
                        Activity responseCard = null;
                        var text = "";


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

                    case Movies4Anyone.Intent.recommendMovie:

                        lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.recommendMovie.ToString()], "", 1);

                        var movieRecoResult = JsonConvert.DeserializeObject<MovieRecommedation>(lookupJson);
                        text = string.Join(',', movieRecoResult.Results.Select(x => x.Title).ToList());

                        responseCard = MessageFactory.Text(text, text, InputHints.IgnoringInput);
                        await stepContext.Context.SendActivityAsync(responseCard, cancellationToken);
                        break;

                    case Movies4Anyone.Intent.recommendTv:

                        lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.recommendTv.ToString()], "", 1);

                        var tvRecoResult = JsonConvert.DeserializeObject<TVRecommendation>(lookupJson);
                        text = string.Join(',', tvRecoResult.Results.Select(x => x.Title).ToList());

                        responseCard = MessageFactory.Text(text, text, InputHints.IgnoringInput);
                        await stepContext.Context.SendActivityAsync(responseCard, cancellationToken);
                        break;

                    case Movies4Anyone.Intent.movieDetails:

                        lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.movieDetails.ToString()], "", 1);

                        var movieDetails = JsonConvert.DeserializeObject<MovieDetails>(lookupJson);
                        text = movieDetails.Title;

                        responseCard = MessageFactory.Text(text, text, InputHints.IgnoringInput);
                        await stepContext.Context.SendActivityAsync(responseCard, cancellationToken);
                        break;

                    case Movies4Anyone.Intent.tvDetails:

                        lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.tvDetails.ToString()], "", 1);

                        var tvDetails = JsonConvert.DeserializeObject<TVDetails>(lookupJson);
                        text = tvDetails.Name;

                        responseCard = MessageFactory.Text(text, text, InputHints.IgnoringInput);
                        await stepContext.Context.SendActivityAsync(responseCard, cancellationToken);
                        break;

                    case Movies4Anyone.Intent.movieVideos:

                        lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.movieVideos.ToString()], "", 1);

                        var movieVideos = JsonConvert.DeserializeObject<Videos>(lookupJson);
                        text = movieVideos.Results.Where(x => x.Type.Equals("Trailer")).FirstOrDefault()?.Key;

                        responseCard = MessageFactory.Text(text, text, InputHints.IgnoringInput);
                        await stepContext.Context.SendActivityAsync(responseCard, cancellationToken);
                        break;

                    case Movies4Anyone.Intent.tvVideos:

                        lookupJson = await _tmdbService
                            .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.tvVideos.ToString()], luisResult.Entities.person.FirstOrDefault(), null);

                        var tvVideos = JsonConvert.DeserializeObject<Videos>(lookupJson);
                        text = tvVideos.Results.Where(x => x.Type.Equals("Trailer")).FirstOrDefault()?.Key;

                        responseCard = MessageFactory.Text(text, text, InputHints.IgnoringInput);
                        await stepContext.Context.SendActivityAsync(responseCard, cancellationToken);
                        break;

                    default:
                        var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                        var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                        await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                        break;
                }

                return await stepContext.NextAsync(null, cancellationToken);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<DialogTurnResult> ResolveIdStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
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

                default:
                    break;
            }


          

            await stepContext.Context.SendActivityAsync(opts.Prompt);
            opts.Prompt = new Activity(type: ActivityTypes.Typing);
            return await stepContext.PromptAsync(nameof(TextPrompt), opts, cancellationToken);

            //if (stepContext.Context.Activity.Value is SearchModel result)
            //{
            //    var promptMessage = MessageFactory.Text($"You have opted for: {result.Id}", InputHints.ExpectingInput);
            //    return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            //}

            //return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
          
            var promptMessage = "I hope this information help you.";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
