
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

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
        public MainDialog(MovieRecognizer luisRecognizer, BookingDialog bookingDialog, ILogger<MainDialog> logger, TMDBService tmdbService)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            _tmdbService = tmdbService;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var test0 = await _tmdbService.FetchInformation("/search/tv", "witcher", null);
            var test = await _tmdbService.FetchInformation("/tv/{id}", "", JsonConvert.DeserializeObject<SearchResult<TvResult>>(test0).Results.First().Id);
            var final = JsonConvert.DeserializeObject<TVDetails>(test);
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            var messageText = stepContext.Options?.ToString() ?? "Yo Yo Yo\nWhat can I help you with today?\nI know a lot about movies and tv shows!\nDon't be afraid to ask :)";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                return await stepContext.BeginDialogAsync(nameof(BookingDialog), new BookingDetails(), cancellationToken);
            }

            var luisResult = await _luisRecognizer.RecognizeAsync<Movies4Anyone>(stepContext.Context, cancellationToken);
         
            switch (luisResult.TopIntent().intent)
            {
                case Movies4Anyone.Intent.searchMovie:
                  
                    var lookupJson = await _tmdbService
                        .FetchInformation(TMDBEndpoints.APIEndpoints[Movies4Anyone.Intent.searchMovie.ToString()], luisResult.Entities.movie.FirstOrDefault(), null);

                    var searchResult = JsonConvert.DeserializeObject<SearchResult<MovieResult>>(lookupJson);
                    var text = string.Join(',', searchResult.Results.Select(x => x.OriginalTitle).ToList());

                    var getWeatherMessage = MessageFactory.Text(text, text, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage, cancellationToken);
                    break;

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
            // If the child dialog ("BookingDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            if (stepContext.Result is BookingDetails result)
            {
                // Now we have all the booking details call the booking service.

                // If the call to the booking service was successful tell the user.

                var timeProperty = new TimexProperty(result.TravelDate);
                var travelDateMsg = timeProperty.ToNaturalLanguage(DateTime.Now);
                var messageText = $"I have you booked to {result.Destination} from {result.Origin} on {travelDateMsg}";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }

            // Restart the main dialog with a different message the second time around
            var promptMessage = "What else can I do for you?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
