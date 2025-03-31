using MealMate.DAL.Entities.Chatbot;

namespace MealMate.DAL.IRepositories.Chatbot
{
    public interface IRecipeRepository
    {
        Task<List<Recipe>> GetAllRecipesAsync();
    }
}
