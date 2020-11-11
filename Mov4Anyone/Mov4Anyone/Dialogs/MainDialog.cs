
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
        public MainDialog(MovieRecognizer luisRecognizer, 
            SearchDialog searchDialog, 
            RecommendationDialog recommendationDialog,
            DetailsDialog detailsDialog,
            VideoDialog videoDialog,
            ILogger<MainDialog> logger, 
            TMDBService tmdbService)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            _tmdbService = tmdbService;
            Logger = logger;

            AddDialog(searchDialog);
            AddDialog(recommendationDialog);
            AddDialog(detailsDialog);
            AddDialog(videoDialog);
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt("Prompt", AdaptiveCardVerifier));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
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

            switch (luisResult.TopIntent().intent)
            {

                case Movies4Anyone.Intent.searchMovie:
                case Movies4Anyone.Intent.searchTV:
                case Movies4Anyone.Intent.searchPeople:
                    return await stepContext.BeginDialogAsync(nameof(SearchDialog), luisResult, cancellationToken);

                case Movies4Anyone.Intent.recommendMovie:
                case Movies4Anyone.Intent.recommendTv:
                    return await stepContext.BeginDialogAsync(nameof(RecommendationDialog), luisResult, cancellationToken);

                case Movies4Anyone.Intent.movieDetails:
                case Movies4Anyone.Intent.tvDetails:
                    return await stepContext.BeginDialogAsync(nameof(DetailsDialog), luisResult, cancellationToken);

                case Movies4Anyone.Intent.movieVideos:
                case Movies4Anyone.Intent.tvVideos:
                    return await stepContext.BeginDialogAsync(nameof(VideoDialog), luisResult, cancellationToken);
                    
                default:
                    var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }


        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var promptMessage = "I hope this information help you.";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
