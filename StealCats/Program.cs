using Application.Services;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Handlers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StealCats.Extensions;
using System.Text.Json.Serialization;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StealCatApiProject", Version = "v1" });
});

builder.Services.AddDbContext<CatContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICatRepository, CatRepository>();
builder.Services.AddScoped<CatService>();
builder.Services.AddHttpClient<IStealCatApiService, StealCatApiService>(client =>
{
    var baseUrl = builder.Configuration["StealCatApi:BaseUrl"];
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new InvalidOperationException("Base URL is not configured.");
    }
    client.BaseAddress = new Uri(baseUrl);
    
});
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StealCatApiProject v1");
        c.RoutePrefix =  "swagger"; 
    });
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<IExceptionHandler>();
        if (await exceptionHandler.TryHandleAsync(context, ex, context.RequestAborted))
        {
            return;
        }
        throw;
    }
});

app.MapStealCatApiEndpoints();

app.Run();