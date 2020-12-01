using DoggoRecognizer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DoggoRecognizer.Services
{
    public class CustomVisionService
    {
        private readonly string _url;
        private readonly string _key;
       
        public CustomVisionService(IConfiguration configuration)
        {
            _url = configuration["PredictionUrl"];
            _key = configuration["PredictionKey"];
        }

        public async Task<string> PredictBreed(IFormFile file)
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(_url),
            };

            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StreamContent(file.OpenReadStream())
                {
                    Headers =
                    {
                        ContentLength = file.Length,
                        ContentType = new MediaTypeHeaderValue(file.ContentType)
                    }
                }, "File", fileName);
                content.Headers.Add("Prediction-Key", _key);
                var response = await client.PostAsync(client.BaseAddress, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<CustomVisionPredictionResult>(response.Content.ReadAsStringAsync().Result);
                    if (!result.Predictions.Any(x => x.Probability >= 0.5))
                    {
                        return "None";
                    }
                    return result.Predictions
                        .FirstOrDefault()
                        .TagName;
                }
            }

            return string.Empty;
        }
    }
}
