
using Newtonsoft.Json;

namespace Mov4Anyone.Models
{
    public class MovieDetails
    {
        [JsonProperty("adult")]
        public bool IsAdult { get; set; }
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
        [JsonProperty("belongs_to_collection")]
        public BelongsToCollection BelongsToCollection { get; set; }
        public int Budget { get; set; }
        public Genre[] Genres { get; set; }
        public string Homepage { get; set; }
        public int Id { get; set; }
        [JsonProperty("imdb_id")]
        public string ImdbId { get; set; }
        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }
        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }
        public string Overview { get; set; }
        public float Popularity { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
        [JsonProperty("production_companies")]
        public ProductionCompanies[] ProductionCompanies { get; set; }
        [JsonProperty("production_countries")]
        public ProductionCountries[] ProductionCountries { get; set; }
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
        public decimal Revenue { get; set; }
        public int Runtime { get; set; }
        [JsonProperty("spoken_languages")]
        public SpokenLanguages[] SpokenLanguages { get; set; }
        public string Status { get; set; }
        public string Tagline { get; set; }
        public string Title { get; set; }
        public bool Video { get; set; }
        [JsonProperty("vote_average")]
        public float VoteAverage { get; set; }
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }
    }

    public class BelongsToCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductionCompanies
    {
        public int Id { get; set; }
        [JsonProperty("logo_path")]
        public string LogoPath { get; set; }
        public string Name { get; set; }
        [JsonProperty("origin_country")]
        public string OriginCountry { get; set; }
    }

    public class ProductionCountries
    {
        [JsonProperty("iso_3166_1")]
        public string Iso3166_1 { get; set; }
        public string Name { get; set; }
    }

    public class SpokenLanguages
    {
        [JsonProperty("iso_639_1")]
        public string Iso639_1 { get; set; }
        public string Name { get; set; }
    }

}

