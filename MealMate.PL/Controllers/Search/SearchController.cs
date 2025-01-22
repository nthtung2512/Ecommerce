using MealMate.BLL.IServices.Search;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers.Search
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly IElasticSearchService _elasticSearchService;

        public SearchController(IElasticSearchService elasticSearchService)
        {
            _elasticSearchService = elasticSearchService;
        }

        [HttpPost("index")]
        public async Task<IActionResult> IndexProducts()
        {
            await _elasticSearchService.IndexProductsAsync();
            return Ok("Products indexed successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> SearchProducts([FromQuery] string query)
        {
            var results = await _elasticSearchService.SearchAsync(query);
            return Ok(results);
        }
    }
}
