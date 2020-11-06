using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mov4Anyone.Models
{
    public class MovieRecommedation
    {
        public int Page { get; set; }
        public RecommendedMovie[] Results { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }

    public class RecommendedMovie
    {
        public int Id { get; set; }
        public bool Video { get; set; }
        [JsonProperty("video_count")]
        public int VoteCount { get; set; }
        [JsonProperty("video_average")]
        public float VoteAverage { get; set; }
        public string Title { get; set; }
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }
        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }
        [JsonProperty("genre_ids")]
        public int[] GenreIds { get; set; }
        public string BackdropPath { get; set; }
        public bool Adult { get; set; }
        public string Overview { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
        public float Popularity { get; set; }
    }

}
