using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace StealCats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : Controller
    {
      
       
       
            private readonly CatService _catService;

            public CatsController(CatService catService)
            {
                _catService = catService;
            }

            [HttpPost("fetch")]
             public async Task<IActionResult> FetchCats([FromQuery] int limit = 25,  [FromQuery] int hasBreeds = 1  )
            {
                await _catService.FetchAndStoreCatsAsync(limit, hasBreeds);
                return Ok();
            }

        [HttpGet("{id}")]
            public async Task<IActionResult> GetCat(int id)
            {
                var cat = await _catService.GetCatByIdAsync(id);
                if (cat == null) return NotFound();
                return Ok(cat);
            }

            [HttpGet]
            public async Task<IActionResult> GetCats(int page = 1, int pageSize = 10)
            {
                var cats = await _catService.GetCatsAsync(page, pageSize);
                return Ok(cats);
            }

            [HttpGet("tag")]
            public async Task<IActionResult> GetCatsByTag(string tag, int page = 1, int pageSize = 10)
            {
                var cats = await _catService.GetCatsByTagAsync(tag, page, pageSize);
                return Ok(cats);
            }
        }
    }


