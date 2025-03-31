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
            Description = "Creates a new rating for a specified recipe. \n\n" +
                          "**Request Body:**\n" +
                          "- entity (RecipeRatingCreateDto) - The data transfer object containing the rating details (required)"
        )]
        [SwaggerResponse(200, "Successfully created the recipe rating", typeof(void))]
        public async Task<IActionResult> CreateRecipeRating(RecipeRatingCreateDto entity)
        {
            await _recipeRatingAppService.CreateAsync(entity);
            return Ok(new { data = true });
        }

        [HttpPut("rating")]
        [SwaggerOperation(
            Summary = "Update recipe rating",
            Description = "Updates an existing rating for a specified recipe. \n\n" +
                          "**Request Body:**\n" +
                          "- entity (RecipeRatingCreateDto) - The data transfer object containing the updated rating details (required)"
        )]
        [SwaggerResponse(200, "Successfully updated the recipe rating", typeof(void))]
        public async Task<IActionResult> UpdateRecipeRating(RecipeRatingCreateDto entity)
        {
            await _recipeRatingAppService.UpdateAsync(entity);
            return Ok();
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
            await _recipeRatingAppService.DeleteAsync(recipeId, customerId);
            return Ok();
        }
    }
}
