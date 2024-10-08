﻿using Application.Services;
using Core.DTOs;
using FluentValidation;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using System.Net;


namespace StealCatServiceTests
{
    public class StealCatsServiceTests

    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IValidator<CatApiResponse> _validator;

        public StealCatsServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://api.thecatapi.com/v1/images/search")
            };

            var inMemorySettings = new Dictionary<string, string> {
            {"CatApi:ApiKey", "test_api_key"},
            {"CatApi:BaseUrl", "https://api.thecatapi.com/v1/images/search"}
        };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
        [Fact]
        public async Task FetchCatImagesAsync_ReturnsCatEntities()
        {
            // Arrange
    

            var searchUrl = "https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1&api_key=test_api_key";
            var imageUrl = "https://cdn2.thecatapi.com/images/1.jpg";

            var responseContent = new JArray(
                new JObject(
               
                    new JProperty("id", "123"), 
                    new JProperty("width", 500),
                    new JProperty("height", 400),
                    new JProperty("url", imageUrl),
                    new JProperty("breeds", new JArray(
                        new JObject(new JProperty("temperament", "Friendly, Playful"))
                    ))
                )
            ).ToString();

           
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("search")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

           
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Equals(imageUrl)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new ByteArrayContent(new byte[] { 1, 2, 3 })
                });

            var catService = new StealCatApiService(_httpClient, _configuration,_validator);

          
            var result = await catService.StealCatsAsync();

          
            Assert.NotNull(result);
            Assert.Single(result);
            var cat = result[0];
         
            Assert.Equal("123", cat.CatId); // Verify CatId
            Assert.Equal(500, cat.Width);
            Assert.Equal(400, cat.Height);
            Assert.NotNull(cat.Image);
            Assert.Equal(new byte[] { 1, 2, 3 }, cat.Image);
            Assert.Equal("Friendly", cat.CatTags[0].Tag.Name);
            Assert.Equal("Playful", cat.CatTags[1].Tag.Name);
        }


    [Fact]
        public async Task FetchCatImagesAsync_ThrowsException_WhenResponseIsInvalid()
        {
            // Arrange
            var responseContent = new JArray(
                new JObject(
                    new JProperty("id", null)
                )
            ).ToString();

            _httpMessageHandlerMock.Protected()
         .Setup<Task<HttpResponseMessage>>(
             "SendAsync",
             ItExpr.IsAny<HttpRequestMessage>(),
             ItExpr.IsAny<CancellationToken>()
         )
         .ReturnsAsync(new HttpResponseMessage
         {
             StatusCode = HttpStatusCode.OK,
             Content = new StringContent(responseContent)
         });

            var catService = new StealCatApiService(_httpClient, _configuration, _validator);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await catService.StealCatsAsync());
        }
    }
}

