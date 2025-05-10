using MealMate.BLL.Dtos.Chatbot;
using MealMate.BLL.IServices.Chatbot;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeAppService _recipeAppService;
        private readonly IRecipeRatingAppService _recipeRatingAppService;

        public RecipeController(IRecipeAppService recipeAppService, IRecipeRatingAppService recipeRatingAppService)
        {
            _recipeAppService = recipeAppService;
            _recipeRatingAppService = recipeRatingAppService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all recipes",
            Description = "Retrieves a list of all available recipes."
        )]
        [SwaggerResponse(200, "Successfully retrieved the list of recipes", typeof(List<RecipeReturnDto>))]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeAppService.GetAllRecipesAsync();
            return Ok(recipes);
        }

        [HttpGet("{recipeId}")]
        [SwaggerOperation(
                       Summary = "Get recipe by ID",
                       Description = "Retrieves a specific recipe by its ID. \n\n" +
                                     "**Parameters:**\n" +
                                     "- `recipeId` (Guid) - The ID of the recipe (required)"
                   )]
        [SwaggerResponse(200, "Successfully retrieved the recipe", typeof(RecipeReturnDto))]
        public async Task<IActionResult> GetRecipeById(Guid recipeId)
        {
            var recipe = await _recipeAppService.GetRecipeByIdAsync(recipeId);
            return Ok(recipe);
        }

        [HttpGet("rating")]
        [SwaggerOperation(
                       Summary = "Get all recipe ratings",
                       Description = "Retrieves a list of all recipe ratings."
                   )]
        [SwaggerResponse(200, "Successfully retrieved the list of recipe ratings", typeof(List<RecipeRatingReturnDto>))]
        public async Task<IActionResult> GetAllRecipeRatings()
        {
            var recipeRatings = await _recipeRatingAppService.GetAllRecipeRatingsAsync();
            return Ok(recipeRatings);
        }

        [HttpGet("rating/{customerId}")]
        [SwaggerOperation(
                       Summary = "Get all recipe ratings by customer ID",
                       Description = "Retrieves a list of all recipe ratings given by a specific customer. \n\n" +
                                     "**Parameters:**\n" +
                                     "- `customerId` (Guid) - The ID of the customer (required)"
                   )]
        [SwaggerResponse(200, "Successfully retrieved the list of recipe ratings by customer ID", typeof(List<RecipeRatingReturnDto>))]
        public async Task<IActionResult> GetAllRecipeRatingsByCustomerId(Guid customerId)
        {
            var recipeRatings = await _recipeRatingAppService.GetAllRecipeRatingsByCustomerIdAsync(customerId);
            return Ok(recipeRatings);
        }

        [HttpGet("rating/{recipeId}/{customerId}")]
        [SwaggerOperation(
            Summary = "Get a recipe rating",
            Description = "Retrieves the rating of a specific recipe by a given customer. \n\n" +
                          "**Parameters:**\n" +
                          "- `recipeId` (Guid) - The ID of the recipe (required)\n" +
                          "- `customerId` (Guid) - The ID of the customer (required)"
        )]
        [SwaggerResponse(200, "Successfully retrieved the recipe rating", typeof(RecipeRatingReturnDto))]
        public async Task<IActionResult> GetRecipeRating(Guid recipeId, Guid customerId)
        {
            var recipeRating = await _recipeRatingAppService.GetRecipeRatingAsync(recipeId, customerId);
            return Ok(recipeRating);
        }

        [HttpPost("rating")]
        [SwaggerOperation(
            Summary = "Create recipe rating",
            Description = "Creates a new rating for a specified recipe.\n\n" +
                          "**Request Body (RecipeRatingCreateDto):**\n" +
                          "- `CustomerId` (Guid) - The ID of the customer submitting the rating. (required)\n" +
                          "- `RecipeId` (Guid) - The ID of the recipe being rated. (required)\n" +
                          "- `Rating` (decimal) - The rating value (e.g., 1.0 to 5.0). (required)"
        )]
        [SwaggerResponse(200, "Successfully created the recipe rating", typeof(void))]
        public async Task<IActionResult> CreateRecipeRating([FromBody] RecipeRatingCreateDto entity)
        {
            var recipeReturnDto = await _recipeRatingAppService.CreateRecipeRatingAsync(entity);
            return Ok(recipeReturnDto);
        }

        [HttpPut("rating")]
        [SwaggerOperation(
            Summary = "Update recipe rating",
            Description = "Updates an existing rating for a specified recipe.\n\n" +
                          "**Request Body (RecipeRatingCreateDto):**\n" +
                          "- `CustomerId` (Guid) - The ID of the customer updating the rating. (required)\n" +
                          "- `RecipeId` (Guid) - The ID of the recipe whose rating is being updated. (required)\n" +
                          "- `Rating` (decimal) - The updated rating value (e.g., 1.0 to 5.0). (required)"
        )]
        [SwaggerResponse(200, "Successfully updated the recipe rating", typeof(void))]
        public async Task<IActionResult> UpdateRecipeRating([FromBody] RecipeRatingCreateDto entity)
        {
            var recipeReturnDto = await _recipeRatingAppService.UpdateRecipeRatingAsync(entity);
            return Ok(recipeReturnDto);
        }

        [HttpDelete("rating/{recipeId}/{customerId}")]
        [SwaggerOperation(
            Summary = "Delete recipe rating",
            Description = "Deletes a specific rating for a specified recipe given by a customer. \n\n" +
                          "**Parameters:**\n" +
                          "- recipeId (Guid) - The ID of the recipe (required)\n" +
                          "- customerId (Guid) - The ID of the customer (required)"
        )]
        [SwaggerResponse(200, "Successfully deleted the recipe rating", typeof(void))]
        public async Task<IActionResult> DeleteRecipeRating(Guid recipeId, Guid customerId)
        {
            var recipeReturnDto = await _recipeRatingAppService.DeleteRecipeRatingAsync(recipeId, customerId);
            return Ok(recipeReturnDto);
        }
    }
}
