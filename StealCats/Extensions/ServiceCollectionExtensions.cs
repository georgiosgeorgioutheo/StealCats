using Application.Services;
using Application.Validations;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.Handlers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Diagnostics;

namespace StealCats.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register repositories
            services.AddScoped<ICatRepository, CatRepository>();

            // Register services
            services.AddScoped<CatService>();

            // Register HTTP clients
            services.AddHttpClient<IStealCatApiService, StealCatApiService>(client =>
            {
                var baseUrl = configuration["StealCatApi:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new InvalidOperationException("Base URL is not configured.");
                }
                client.BaseAddress = new Uri(baseUrl);
            });

            // Register exception handler
            services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

            // Register validators
            services.AddTransient<IValidator<CatApiResponse>, CatApiResponseValidator>();
            services.AddTransient<IValidator<CatResponseDto>, CatResponseDtoValidator>();
            services.AddTransient<IValidator<CatEntity>, CatEntityValidator>();
            services.AddTransient<IValidator<(int page, int pageSize)>, CatRequestValidator>();
        }
    }
}
