
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mov4Anyone.Models
{

    public class SearchResult<T>
    {
        public int Page { get; set; }
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        public IEnumerable<T> Results { get; set; }
    }

    public class MovieResult
    {
        public float Popularity { get; set; }
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }
        public bool Video { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
        public int Id { get; set; }
        public bool Adult { get; set; }
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }
        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }
        [JsonProperty("genre_ids")]
        public int?[] GenreIds { get; set; }
        public string Title { get; set; }
        [JsonProperty("vote_average")]
        public float VoteAverage { get; set; }
        public string Overview { get; set; }
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
    }


    public class TvResult
    {
        [JsonProperty("original_name")]
        public string OriginalName { get; set; }
        [JsonProperty("genre_ids")]
        public int[] GenreIds { get; set; }
        public string Name { get; set; }
        public float Popularity { get; set; }
        [JsonProperty("origin_country")]
        public string[] OriginCountry { get; set; }
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }
        [JsonProperty("first_air_date")]
        public string FirstAirDate { get; set; }
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }
        public int Id { get; set; }
        [JsonProperty("vote_average")]
        public float VoteAverage { get; set; }
        public string Overview { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
    }


    public class PersonResult
    {
        public float Popularity { get; set; }
        [JsonProperty("known_for_department")]
        public string KnownForDepartment { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        [JsonProperty("profile_path")]
        public string ProfilePath { get; set; }
        public bool Adult { get; set; }
        [JsonProperty("known_for")]
        public PersonKnownFor[] KnownFor { get; set; }
        public int Gender { get; set; }
    }

    public class PersonKnownFor
    {
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }
        public bool Video { get; set; }
        [JsonProperty("media_type")]
        public string MediaType { get; set; }
        public int Id { get; set; }
        public bool Adult { get; set; }
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }
        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }
        [JsonProperty("genre_ids")]
        public int[] GenreIds { get; set; }
        public string Title { get; set; }
        [JsonProperty("vote_average")]
        public float VoteAverage { get; set; }
        public string Overview { get; set; }
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
    }


}
