using MealMate.BLL.Dtos.Chatbot;
using MealMate.BLL.IServices.Chatbot;
using MealMate.DAL.IRepositories.Chatbot;
using MealMate.DAL.Utils.Exceptions;

namespace MealMate.BLL.Services.Chatbot
{
    internal class RecipeAppService : IRecipeAppService
    {
        private readonly IRecipeRepository _recipeRepository;
        public RecipeAppService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }
        public async Task<List<RecipeReturnDto>> GetAllRecipesAsync()
        {
            var recipes = await _recipeRepository.GetAllRecipesAsync()
                ?? throw new EntityNotFoundException("No recipes found");

            return recipes.Select(recipe => new RecipeReturnDto
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Tags = recipe.GetListTags().ToList(),
                Image = recipe.Image,
                Instructions = recipe.Instructions,
                Summary = recipe.Summary,
                ReadyInMinutes = recipe.ReadyInMinutes,
                Servings = recipe.Servings,
                HealthScore = recipe.HealthScore,
                AverageRating = recipe.AverageRating,
                Ingredients = recipe.Ingredients.Select(i => new IngredientReturnDto
                {
                    RecipeId = i.RecipeId,
                    Name = i.Name,
                    Amount = i.Amount,
                    Unit = i.Unit,
                    Original = i.Original,
                    NameClean = i.NameClean
                }).ToList()
            }).ToList();
        }

        public async Task<RecipeReturnDto> GetRecipeByIdAsync(Guid recipeId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId)
                ?? throw new EntityNotFoundException("Recipe not found");

            return new RecipeReturnDto
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Tags = recipe.GetListTags().ToList(),
                Image = recipe.Image,
                Instructions = recipe.Instructions,
                Summary = recipe.Summary,
                ReadyInMinutes = recipe.ReadyInMinutes,
                Servings = recipe.Servings,
                HealthScore = recipe.HealthScore,
                AverageRating = recipe.AverageRating,
                Ingredients = recipe.Ingredients.Select(i => new IngredientReturnDto
                {
                    RecipeId = i.RecipeId,
                    Name = i.Name,
                    Amount = i.Amount,
                    Unit = i.Unit,
                    Original = i.Original,
                    NameClean = i.NameClean
                }).ToList()
            };
        }

    }
}
