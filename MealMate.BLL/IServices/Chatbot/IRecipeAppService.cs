using MealMate.BLL.Dtos.Chatbot;

namespace MealMate.BLL.IServices.Chatbot
{
    public interface IRecipeAppService
    {
        Task<List<RecipeReturnDto>> GetAllRecipesAsync();
        Task<RecipeReturnDto> GetRecipeByIdAsync(Guid recipeId);
    }
}
