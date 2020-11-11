using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mov4Anyone.Models
{
    public class TVRecommendation
    {
        public int Page { get; set; }
        public RecommendedShow[] Results { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }

    public class RecommendedShow
    {
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
        [JsonProperty("first_air_date")]
        public string FirstAirDate { get; set; }
        [JsonProperty("genre_ids")]
        public int[] GenreIds { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("origin_country")]
        public string[] OriginCountry { get; set; }
        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }
        [JsonProperty("original_name")]
        public string OriginalName { get; set; }
        public string Overview { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
        [JsonProperty("vote_average")]
        public float VoteAverage { get; set; }
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }
        public Channel[] Networks { get; set; }
        public float Popularity { get; set; }
    }

    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("origin_country")]
        public string OriginCountry { get; set; }
    }
}
