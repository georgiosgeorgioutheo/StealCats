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
    public class StealCatApiService : IStealCatApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public StealCatApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["CatApi:BaseUrl"];
            _apiKey = configuration["CatApi:ApiKey"];
        }

        //public CatApiService(HttpClient httpClient, IConfiguration configuration)
        //{
        //    _httpClient = httpClient;
        //    _apiKey = configuration["CatApi:ApiKey"];
        //    _baseUrl = configuration["CatApi:BaseUrl"];
        //    _apiUrl = "https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1&api_key=live_lMH4324itjRC2C3aMinSKqgs6VJ7whDqJsDzG1UxWsS2QEO4uJJVl5JcgVepNMrk";
        //}


        public async Task<List<CatEntity>> FetchCatImagesAsync(int limit = 25, int hasBreeds = 1)
        {
            // Construct the query parameters
            var queryParams = new List<string>
            {
                $"limit={limit}",
                $"has_breeds={hasBreeds}",
                $"api_key={_apiKey}"
            };

            var queryString = string.Join("&", queryParams);
            var response = await _httpClient.GetStringAsync($"{_baseUrl}?{queryString}");
            var catList = JArray.Parse(response).ToObject<List<JObject>>();

            var result = new List<CatEntity>();

            foreach (var jObject in catList)
            {
                var cat = await CreateCatEntityAsync(jObject);
                AddTagsToCatEntity(jObject, cat);
                result.Add(cat);
            }

            return result;

        }


        private async Task<CatEntity> CreateCatEntityAsync(JObject jObject)
        {
            return new CatEntity
            {
                CatId = jObject["id"].ToString(),
                Width = (int)jObject["width"],
                Height = (int)jObject["height"],
                Image = await _httpClient.GetByteArrayAsync(jObject["url"].ToString()),
                Created = DateTime.UtcNow,
                CatTags = new List<CatTagEntity>()
            };
        }

        private void AddTagsToCatEntity(JObject jObject, CatEntity cat)
        {
            if (jObject["breeds"] != null)
            {
                foreach (var breed in jObject["breeds"])
                {
                    var tags = ExtractTagsFromBreed(breed);

                    foreach (var tag in tags)
                    {
                        var tagEntity = new TagEntity
                        {
                            Name = tag,
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
        }

        private List<string> ExtractTagsFromBreed(JToken breed)
        {
            var tags = breed["temperament"].ToString().Split(',');
            var trimmedTags = new List<string>();

            foreach (var tag in tags)
            {
                trimmedTags.Add(tag.Trim());
            }

            return trimmedTags;
        }
    }
}
           