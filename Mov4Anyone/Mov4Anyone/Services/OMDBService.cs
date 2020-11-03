using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mov4Anyone.Models;
using Newtonsoft.Json;

namespace Mov4Anyone.Services
{
    public class OMDBService
    {
        private readonly string _baseUrl = "";
        private readonly Dictionary<string, string> _paramMap = new Dictionary<string, string>()
        {
            {"title", "t="},
            {"shortPlot", "plot=short"},
            {"fullPlot", "plot=full"},
            {"year", "y="},
            {"type", "type="},
            {"search", "s="},
            {"page", "page="}
        };

        public OMDBService(IConfiguration configuration)
        {
            _baseUrl = $"{configuration["OMDB_Url"]}?{configuration["OMDB_Key"]}";
        }

        public async Task<OMDBMovie> FetchInfoForMovie()
        {
            var movieInfo = new OMDBMovie();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl + "&t=Avengers");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync(client.BaseAddress);

                if (Res.IsSuccessStatusCode)
                {
                    var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                    movieInfo = JsonConvert.DeserializeObject<OMDBMovie>(ObjResponse);
                }
                
                return movieInfo;
            }
        }

        public async Task<OMDBSearch> SearchForMovie()
        {
            var movieInfo = new OMDBSearch();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/ProjectA/GetDepartments");

                if (Res.IsSuccessStatusCode)
                {
                    var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                    movieInfo = JsonConvert.DeserializeObject<OMDBSearch>(ObjResponse);
                }

                return movieInfo;
            }
        }
    }
}
