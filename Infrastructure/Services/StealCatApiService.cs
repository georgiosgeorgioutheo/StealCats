using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services
{
    public class StealCatApiService : IStealCatApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _baseUrl;
        private readonly string? _apiKey;

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
            var queryString = ConstructQueryString(limit, hasBreeds);

            // Fetch and parse the cat list
            var catList = await FetchCatListAsync(queryString);

            var result = new List<CatEntity>();
            if (catList != null)
            {
                foreach (var jObject in catList)
                {
                    var cat = await CreateCatEntityAsync(jObject);
                    AddTagsToCatEntity(jObject, cat);
                    result.Add(cat);
                }
            }
            return result;
        }

        private string ConstructQueryString(int limit, int hasBreeds)
        {
            var queryParams = new List<string>
            {
            $"limit={limit}",
            $"has_breeds={hasBreeds}",
            $"api_key={_apiKey}"
            };

            return string.Join("&", queryParams);
        }
        private async Task<List<JObject>> FetchCatListAsync(string queryString)
        {
            var response = await _httpClient.GetStringAsync($"{_baseUrl}?{queryString}");
            var catList = JArray.Parse(response).ToObject<List<JObject>>();
            return catList ?? new List<JObject>();
        }
        //public async Task<List<CatEntity>> FetchCatImagesAsync(int limit = 25, int hasBreeds = 1)
        //{
        //    // Construct the query parameters
        //    var queryParams = new List<string>
        //    {
        //        $"limit={limit}",
        //        $"has_breeds={hasBreeds}",
        //        $"api_key={_apiKey}"
        //    };

        //    var queryString = string.Join("&", queryParams);
        //    var response = await _httpClient.GetStringAsync($"{_baseUrl}?{queryString}");
        //    var catList = JArray.Parse(response).ToObject<List<JObject>>();

        //    var result = new List<CatEntity>();
        //    if (catList != null)
        //    {
        //        foreach (var jObject in catList)
        //        {
        //            var cat = await CreateCatEntityAsync(jObject);
        //            AddTagsToCatEntity(jObject, cat);
        //            result.Add(cat);
        //        }

        //    }
        //    return result;

        //}


        private async Task<CatEntity> CreateCatEntityAsync(JObject jObject)
        {
            if (jObject["id"] is not JToken idToken ||
            jObject["width"] is not JToken widthToken ||
            jObject["height"] is not JToken heightToken ||
            jObject["url"] is not JToken urlToken)
            {
                // Handle the case where any token is null
                throw new InvalidOperationException("Invalid JSON data.");
            }

            var cat = new CatEntity
            {
                CatId = idToken.ToString(),
                Width = (int)widthToken,
                Height = (int)heightToken,
                Image = await _httpClient.GetByteArrayAsync(urlToken.ToString()),
                Created = DateTime.UtcNow,
                CatTags = new List<CatTagEntity>()
            };

            return cat;
        }

        private void AddTagsToCatEntity(JObject jObject, CatEntity cat)
        {
            if (jObject["breeds"] is JArray breedsArray)
            {                
                foreach (var breed in breedsArray)
                {
                    var tags = ExtractTagsFromBreed(breed);
                    AddTags(cat, tags);
                   
                }
            }
        }

        private List<string> ExtractTagsFromBreed(JToken breed)
        {
            var trimmedTags = new List<string>();
          
            if (breed?["temperament"] != null)
            {
                var temperamentToken = breed["temperament"];

                // Ensure the temperamentToken is not null and convert it to a string safely
                var temperamentString = temperamentToken?.ToString();

                if (!string.IsNullOrEmpty(temperamentString))
                {
                    var tags = temperamentString.Split(',');

                    foreach (var tag in tags)
                    {
                        trimmedTags.Add(tag.Trim());
                    }
                }
            }
            return trimmedTags;
        }

        private void AddTags(CatEntity cat, List<string> tags)
        {
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
}
           