
using Newtonsoft.Json;

namespace DoggoRecognizer.Models
{
    public class DogAPIInfo
    {
        public Stat Weight { get; set; }
        public Stat Height { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
        [JsonProperty("bred_for")]
        public string BredFor { get; set; }
        [JsonProperty("breed_group")]
        public string BreedGroup { get; set; }
        [JsonProperty("life_span")]
        public string LifeSpan { get; set; }
        public string Temperament { get; set; }
        public string Origin { get; set; }
    }

    public class Stat
    {
        public string Imperial { get; set; }
        public string Metric { get; set; }
    }

}
