using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Services
{
    public class CatApiService : ICatApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _apiUrl;
        public CatApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["CatApi:ApiKey"];
            _baseUrl = configuration["CatApi:BaseUrl"];
            _apiUrl = "https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1&api_key=live_lMH4324itjRC2C3aMinSKqgs6VJ7whDqJsDzG1UxWsS2QEO4uJJVl5JcgVepNMrk";
        }

      
            public async Task<List<CatEntity>> FetchCatImagesAsync(int limit = 25, int page = 0, string order = "RAND", int hasBreeds = 1, string breedIds = null, string categoryIds = null, string subId = null)
            {
            // Construct the query parameters
            var queryParams = new List<string>
            {
                $"limit={limit}",
                $"page={page}",
                $"order={order}",
                $"has_breeds={hasBreeds}",
                $"api_key={_apiKey}"
            };

            if (!string.IsNullOrEmpty(breedIds))
                {
                    queryParams.Add($"breed_ids={breedIds}");
                }

                if (!string.IsNullOrEmpty(categoryIds))
                {
                    queryParams.Add($"category_ids={categoryIds}");
                }

                if (!string.IsNullOrEmpty(subId))
                {
                    queryParams.Add($"sub_id={subId}");
                }

                var queryString = string.Join("&", queryParams);
            var response = await _httpClient.GetStringAsync(_apiUrl);
            //   var response = await _httpClient.GetStringAsync($"{_baseUrl}/images/search?{queryString}&api_key={_apiKey}");
            var catList = JArray.Parse(response).ToObject<List<JObject>>();

                var result = new List<CatEntity>();

                foreach (var jObject in catList)
            {
                var cat = new CatEntity
                {
                    CatId = jObject["id"].ToString(),
                    Width = (int)jObject["width"],
                    Height = (int)jObject["height"],
                    Image = await _httpClient.GetByteArrayAsync(jObject["url"].ToString()),
                    Created = DateTime.UtcNow
                };

                result.Add(cat);
            }

            // Map the id from the response to CatId
            
            return result;
        }
    }
}
