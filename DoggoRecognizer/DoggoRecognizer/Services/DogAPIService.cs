using DoggoRecognizer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DoggoRecognizer.Services
{
    public class DogAPIService
    {
        private readonly List<BreedInfo> _breedInfos;

        public DogAPIService(IConfiguration configuration)
        {
            var list = new List<BreedInfo>();
            configuration.GetSection("DogRaceSettings").Bind(list);
            _breedInfos = list;
        }

        public async Task<DogInfoModel> FetchBreedInfo(string breedPrediction)
        {
            DogInfoModel model = new DogInfoModel();

            var breed = _breedInfos.FirstOrDefault(x => x.Name.Equals(breedPrediction));
            
            if (breed.Id == 0)
            {
                model.ApiInfo = new DogAPIInfo();
                model.ApiInfo.Name = breed.Name;
            }
            else
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri($"https://api.thedogapi.com/v1/breeds/{breed.Id}")
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync(client.BaseAddress);


                if (Res.IsSuccessStatusCode)
                {
                    model.ApiInfo = JsonConvert.DeserializeObject<DogAPIInfo>(Res.Content.ReadAsStringAsync().Result);
                }
            }

            model.WikiPath = breed.WikiPage;
            model.BreedId = breed.Id;
            return model;
        }

        public string ChangeImageToBase64(IFormFile file)
        {
            string imageString = "";
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    imageString = Convert.ToBase64String(fileBytes);
                }
            }
            return imageString;
        }
    }
}
