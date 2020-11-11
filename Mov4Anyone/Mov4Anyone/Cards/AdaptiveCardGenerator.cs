﻿
using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Mov4Anyone.Models;
using Mov4Anyone.Services;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace Mov4Anyone.Cards
{
    public class AdaptiveCardGenerator
    {

        public PromptOptions GenerateMovieSearchAttachment(SearchResult<MovieResult> movieResults)
        {

            var paths = new[] { ".", "Cards" ,"searchTemplate.json" };
            
            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            AdaptiveCardParseResult result = AdaptiveCard.FromJson(adaptiveCardJson);
            AdaptiveCard card = result.Card;

            var title = (AdaptiveTextBlock)card.Body.First();
            title.Text = "Did you mean any of these movies?";

            card.Actions = movieResults.Results
                .OrderByDescending(x => x.Popularity)
                .Take(6)
                .Select(x => new AdaptiveSubmitAction
                {
                    Title = $"{x.Title} ({x.ReleaseDate?.Split("-")[0]})",
                    Data = new {
                        x.Id,
                        Type = "Movie",
                        KnownFor = new PersonKnownFor[1]
                    }
                })
                .ToList<AdaptiveAction>();

            return new PromptOptions
            {
                Prompt = (Activity)MessageFactory.Attachment(new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JObject.FromObject(card)
                })
            }; 
        }

        public PromptOptions GenerateTvSearchAttachment(SearchResult<TvResult> movieResults)
        {

            var paths = new[] { ".", "Cards", "searchTemplate.json" };

            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            AdaptiveCardParseResult result = AdaptiveCard.FromJson(adaptiveCardJson);
            AdaptiveCard card = result.Card;

            var title = (AdaptiveTextBlock)card.Body.First();
            title.Text = "Did you mean any of these tv shows?";

            card.Actions = movieResults.Results
                .OrderByDescending(x => x.Popularity)
                .Take(6)
                .Select(x => new AdaptiveSubmitAction
                {
                    Title = $"{x.Name} ({x.FirstAirDate?.Split("-")[0]})",
                    Data = new
                    {
                        x.Id,
                        Type = "Tv",
                        KnownFor = new PersonKnownFor[1]
                    }
                })
                .ToList<AdaptiveAction>();

            return new PromptOptions
            {
                Prompt = (Activity)MessageFactory.Attachment(new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JObject.FromObject(card)
                })
            };
        }

        public PromptOptions GeneratePersonSearchAttachment(SearchResult<PersonResult> movieResults)
        {

            var paths = new[] { ".", "Cards", "searchTemplate.json" };

            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            AdaptiveCardParseResult result = AdaptiveCard.FromJson(adaptiveCardJson);
            AdaptiveCard card = result.Card;

           var title = (AdaptiveTextBlock)card.Body.First();
            title.Text = "Did you mean any of these people?";

            card.Actions = movieResults.Results
                .OrderByDescending(x => x.Popularity)
                .Take(6)
                .Select(x => new AdaptiveSubmitAction
                {
                    Title = $"{x.Name} ({x.KnownForDepartment})",
                    Data = new
                    {
                        x.Id,
                        Type = "Person",
                        x.KnownFor
                    }
                })
                .ToList<AdaptiveAction>();

            return new PromptOptions
            {
                Prompt = (Activity)MessageFactory.Attachment(new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JObject.FromObject(card)
                })
            };
        }


        public PromptOptions GeneratePersonDetailsAttachment(PersonDetails personDetails, PersonKnownFor[] knownFor)
        {
            var paths = new[] { ".", "Cards", "personDetails.json" };
            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            AdaptiveCardParseResult result = AdaptiveCard.FromJson(adaptiveCardJson);
            AdaptiveCard card = result.Card;

            ((AdaptiveTextBlock)card.Body.First()).Text = "Person details";
            ((AdaptiveImage)((AdaptiveColumnSet)card.Body[1]).Columns.First().Items.First()).Url = new Uri($"https://image.tmdb.org/t/p/w500/{personDetails.profile_path}");

            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items.First()).Text = personDetails.name;
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[1]).Text = $"Born: {personDetails.birthday}";
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[2]).Text = $"Role: {personDetails.known_for_department}";
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[1]).Text = $"Born: {personDetails.birthday}";
            ((AdaptiveTextBlock)card.Body[2]).Text = personDetails.biography;

            ((AdaptiveFactSet)card.Body[4]).Facts
                .AddRange(knownFor.Select(x => new AdaptiveFact
                {
                    Title = x.Title,
                    Value = $"({x.ReleaseDate.Split("-")[0]}) - {TMDBGenres.MovieGenres.Where(y => y.Value == x.GenreIds[0]).FirstOrDefault().Key}"
                }));

            ((AdaptiveOpenUrlAction)card.Actions.First()).Url = new Uri($"https://www.imdb.com/name/{personDetails.imdb_id}");


            return new PromptOptions
            {
                Prompt = (Activity)MessageFactory.Attachment(new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JObject.FromObject(card)
                })
            };
        }

        public PromptOptions GenerateMovieDetailsAttachment(MovieDetails movieDetails)
        {
            var paths = new[] { ".", "Cards", "movieDetails.json" };
            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            AdaptiveCardParseResult result = AdaptiveCard.FromJson(adaptiveCardJson);
            AdaptiveCard card = result.Card;

            ((AdaptiveTextBlock)card.Body.First()).Text = "Movie details";
            ((AdaptiveImage)((AdaptiveColumnSet)card.Body[1]).Columns.First().Items.First()).Url = new Uri($"https://image.tmdb.org/t/p/w500/{movieDetails.PosterPath}");

            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items.First()).Text = movieDetails.Title;
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[1]).Text = $"Released: {movieDetails.ReleaseDate}";
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[2]).Text = $"Genres: {string.Join(", ", movieDetails.Genres.Select(x => x.Name))}";
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[3]).Text = $"Status: {movieDetails.Status}";
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[4]).Text = $"Rating: {movieDetails.VoteAverage}";
            ((AdaptiveTextBlock)card.Body[2]).Text = movieDetails.Overview;

            ((AdaptiveOpenUrlAction)card.Actions.First()).Url = new Uri($"https://www.imdb.com/title/{movieDetails.ImdbId}");
        
            return new PromptOptions
            {
                Prompt = (Activity)MessageFactory.Attachment(new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JObject.FromObject(card)
                })
            };
        }

        public PromptOptions GenerateTVDetailsAttachment(TVDetails tvDetails)
        {
            var paths = new[] { ".", "Cards", "movieDetails.json" };
            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            AdaptiveCardParseResult result = AdaptiveCard.FromJson(adaptiveCardJson);
            AdaptiveCard card = result.Card;

            ((AdaptiveTextBlock)card.Body.First()).Text = "Movie details";
            ((AdaptiveImage)((AdaptiveColumnSet)card.Body[1]).Columns.First().Items.First()).Url = new Uri($"https://image.tmdb.org/t/p/w500/{tvDetails.PosterPath}");

            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items.First()).Text = tvDetails.Name;
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[1]).Text = $"Released: {tvDetails.FirstAirDate}";
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[2]).Text = $"Genres: {string.Join(", ", tvDetails.Genres.Select(x => x.Name))}";
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[3]).Text = $"Status: {tvDetails.Status}";
            ((AdaptiveTextBlock)((AdaptiveColumnSet)card.Body[1]).Columns[1].Items[4]).Text = $"Rating: {tvDetails.VoteAverage}";
            ((AdaptiveTextBlock)card.Body[2]).Text = tvDetails.Overview;

            card.Actions = null;

            ((AdaptiveFactSet)card.Body[3]).Facts
               .AddRange(tvDetails.Seasons.OrderBy(x => x.SeasonNumber).Select(x => new AdaptiveFact
               {
                   Title = x.Name,
                   Value = $"- ({x.AirDate.Split("-")[0]}) - {x.EpisodeCount} episodes"
               }));

            return new PromptOptions
            {
                Prompt = (Activity)MessageFactory.Attachment(new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JObject.FromObject(card)
                })
            };
        }
    }
}