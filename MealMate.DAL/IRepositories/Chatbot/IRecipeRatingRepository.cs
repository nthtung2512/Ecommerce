using MealMate.DAL.Entities.Chatbot;

namespace MealMate.DAL.IRepositories.Chatbot
{
    public interface IRecipeRatingRepository
    {
        Task<List<RecipeRating>> GetAllAsync();
        Task<List<RecipeRating>> GetRecipeRatingByCustomerIdAsync(Guid customerId);
        Task<RecipeRating?> GetRecipeRatingAsync(Guid recipeId, Guid customerId);
        Task<List<RecipeRating>> GetAllRatingByRecipeIdAsync(Guid recipeId);
        Task CreateAsync(RecipeRating entity);
        Task UpdateAsync(RecipeRating entity);
        Task DeleteAsync(RecipeRating entity);
    }
}
