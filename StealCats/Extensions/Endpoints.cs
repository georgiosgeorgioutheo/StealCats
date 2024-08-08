using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Net.Http;

namespace StealCats.Extensions
{
    public static class Endpoints
    {
        public static void MapStealCatApiEndpoints(this IEndpointRouteBuilder app)
        {
            
                app.MapGet("/api/cats", async (HttpContext httpContext, ICatRepository catRepository, int page = 1, int pageSize = 10) =>
                    {
                        var cats = await catRepository.GetCatsAsync(page, pageSize);
                        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
                        var response = cats.Select(cat => ToCatResponse(cat, baseUrl));
                        return Results.Ok(response);
                    })
                    .WithName("GetCats")
                    .WithTags("Cats")
                    .Produces<IEnumerable<Object>>(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status400BadRequest)
                    .Produces(StatusCodes.Status500InternalServerError)
                    .WithOpenApi();

                app.MapGet("/api/cats/{id}", async (HttpContext httpContext, ICatRepository catRepository, int id) =>
                        {
                            var cat = await catRepository.GetCatByIdAsync(id);
                            if (cat == null)
                            {
                                throw new KeyNotFoundException("Cat not found");
                            }
                            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
                            var result = ToCatResponse(cat,baseUrl);
                            return Results.Ok(result);
                        })
                        .WithName("GetCatById")
                        .WithTags("Cats")
                        .Produces<CatEntity>(StatusCodes.Status200OK)
                        .Produces(StatusCodes.Status404NotFound)
                        .Produces(StatusCodes.Status400BadRequest)
                        .Produces(StatusCodes.Status500InternalServerError)
                        .WithOpenApi();


                app.MapGet("/api/cats/tag/{tag}", async (HttpContext httpContext, ICatRepository catRepository, string tag, int page = 1, int pageSize = 10) =>
                {
                    var cats = await catRepository.GetCatsByTagAsync(tag, page, pageSize);
                    var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
                    var response = cats.Select(cat => ToCatResponse(cat, baseUrl));
                    return Results.Ok(response);
                  
                   
                })
                .WithName("GetCatsByTag")
                .WithTags("Cats")
                .Produces<IEnumerable<CatEntity>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi();




                app.MapPost("/api/cats/Steal", async (IStealCatApiService catService, ICatRepository catRepository) =>
                {
                 List<CatEntity> cats = await catService.StealCatsAsync();
                    await catRepository.AddCatsAsync(cats);
                     var responseMessage = new
                    {
                        Message = $"{cats.Count} cats successfully added."
                    };
                    return Results.Ok(responseMessage);
                })
                .WithName("FetchAndStoreCats")
                .WithTags("Cats")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi();




                // Endpoint to return an image as base64-encoded string in JSON
                app.MapGet("/api/cats/{id}/image", async (ICatRepository catRepository, int id) =>
                {
                    var cat = await catRepository.GetCatByIdAsync(id);
                    if (cat == null || cat.Image == null)
                    {
                        throw new KeyNotFoundException("Cat not found");
                    }

                    return Results.File(cat.Image, "image/jpeg");
                })
             .WithName("GetCatImage")
             .WithTags("Cats")
             .Produces(StatusCodes.Status200OK, typeof(FileResult))
                 .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi();
            }

            private static object ToCatResponse(CatEntity cat, string baseUrl)
            {
                return new
                {
                    cat.Id,
                    cat.CatId,
                    cat.Width,
                    cat.Height,
                    cat.Created,
                    ImageUrl = $"{baseUrl}/api/cats/{cat.Id}/image",
                    Tags = cat.CatTags.Select(ct => ct.Tag.Name).ToList()
                };
            }
        }
    }


