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
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly int _limit;
        private readonly int _hasBreeds;

        public CatApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["CatApi:BaseUrl"];
            _apiKey = configuration["CatApi:ApiKey"];
            _limit = int.Parse(configuration["CatApi:Limit"]);
            _hasBreeds = int.Parse(configuration["CatApi:HasBreeds"]);
        }

        //public CatApiService(HttpClient httpClient, IConfiguration configuration)
        //{
        //    _httpClient = httpClient;
        //    _apiKey = configuration["CatApi:ApiKey"];
        //    _baseUrl = configuration["CatApi:BaseUrl"];
        //    _apiUrl = "https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1&api_key=live_lMH4324itjRC2C3aMinSKqgs6VJ7whDqJsDzG1UxWsS2QEO4uJJVl5JcgVepNMrk";
        //}

      
            public async Task<List<CatEntity>> FetchCatImagesAsync(int limit = 25, int page = 0, string order = "RAND", int hasBreeds = 1, string breedIds = null, string categoryIds = null, string subId = null)
            {
            // Construct the query parameters
            var queryParams = new List<string>
            {
                $"limit={_limit}",
                $"has_breeds={_hasBreeds}",
                $"api_key={_apiKey}"
            };

            var queryString = string.Join("&", queryParams);
            var response = await _httpClient.GetStringAsync($"{_baseUrl}?{queryString}");
            var catList = JArray.Parse(response).ToObject<List<JObject>>();

            var result = new List<CatEntity>();



            //var queryString = string.Join("&", queryParams);
            //var response = await _httpClient.GetStringAsync(_apiUrl);
            //   var response = await _httpClient.GetStringAsync($"{_baseUrl}/images/search?{queryString}&api_key={_apiKey}");
            // var catList = JArray.Parse(response).ToObject<List<JObject>>();

            //  var result = new List<CatEntity>();
            foreach (var jObject in catList)
            {
                var cat = new CatEntity
                {
                    CatId = jObject["id"].ToString(),
                    Width = (int)jObject["width"],
                    Height = (int)jObject["height"],
                    Image = await _httpClient.GetByteArrayAsync(jObject["url"].ToString()),
                    Created = DateTime.UtcNow,
                    CatTags = new List<CatTagEntity>()
                };

                if (jObject["breeds"] != null)
                {
                    foreach (var breed in jObject["breeds"])
                    {
                        var tags = breed["temperament"].ToString().Split(',');

                        foreach (var tag in tags)
                        {
                            var trimmedTag = tag.Trim();
                            var tagEntity = new TagEntity
                            {
                                Name = trimmedTag,
                                Created = DateTime.UtcNow
                            };
                            cat.CatTags.Add(new CatTagEntity
                            {
                                Cat = cat,
                                Tag = tagEntity
                            });
                        }
                    }
                }

                result.Add(cat);
            }

            return result;
        }
    }
}
