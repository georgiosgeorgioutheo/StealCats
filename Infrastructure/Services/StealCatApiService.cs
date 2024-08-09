using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Mappings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;


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
            _baseUrl = configuration["StealCatApi:BaseUrl"];
            _apiKey = configuration["StealCatApi:ApiKey"];
        }

   

        public async Task<List<CatEntity>> StealCatsAsync(int limit = 25, int hasBreeds = 1)
        {
            // Construct the query parameters
            var queryString = ConstructQueryString(limit, hasBreeds);

            // Fetch and parse the cat list
            var catList = await FetchCatListAsync(queryString);

            var result = new List<CatEntity>();

            foreach (var catApiResponse in catList)
            {
                var catEntity = await CatMappings.ToCatEntityAsync(catApiResponse,_httpClient);
                result.Add(catEntity);
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
        private async Task<List<CatApiResponse>> FetchCatListAsync(string queryString)
        {
            var response = await _httpClient.GetStringAsync($"{_baseUrl}?{queryString}");   
            var catList = JsonConvert.DeserializeObject<List<CatApiResponse>>(response);
            return catList ?? new List<CatApiResponse>();
        }


    }
}
           