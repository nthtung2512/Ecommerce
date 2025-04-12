using MealMate.DAL.Entities.Chatbot;

namespace MealMate.DAL.IRepositories.Chatbot
{
    public interface IRecipeRepository
    {
        Task<Recipe?> GetByIdAsync(Guid id);
        Task<List<Recipe>> GetAllRecipesAsync();
        Task<Recipe> UpdateAsync(Recipe recipe);
    }
}
