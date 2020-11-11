using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mov4Anyone.Services
{
    public class TMDBService
    {
        protected readonly string _baseUrl = "";
        protected readonly string _apiKey = "";

        public TMDBService(IConfiguration configuration)
        {
            _baseUrl = configuration["TMDB_Url"];
            _apiKey = $"?api_key={configuration["TMDB_Key"]}";
        }

        protected string BuildAPIUrl(string endpoint, string query, int? id)
        {
            string url = _baseUrl;
            if (endpoint.Contains("search"))
            {
                url += $"{endpoint}{_apiKey}&query={query}";
            }
            else
            {
                url += $"{endpoint.Replace("{id}", id.ToString())}{_apiKey}";
            }
            url += "&language=en-US";


            return url;
        }

        public string GetBaseUrl()
        {
            return _baseUrl + _apiKey;
        }


        public async Task<string> FetchInformation(string endpoint, string query, int? id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BuildAPIUrl(endpoint, query, id));
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync(client.BaseAddress);

                if (Res.IsSuccessStatusCode)
                {
                    return Res.Content.ReadAsStringAsync().Result;
                }

                return default;
            }
        }
    }
}
