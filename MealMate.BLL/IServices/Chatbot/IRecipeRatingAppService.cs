using MealMate.BLL.Dtos.Chatbot;

namespace MealMate.BLL.IServices.Chatbot
{
    public interface IRecipeRatingAppService
    {
        Task<RecipeRatingReturnDto> GetRecipeRatingAsync(Guid recipeId, Guid customerId);
        Task CreateAsync(RecipeRatingCreateDto entity);
        Task UpdateAsync(RecipeRatingCreateDto entity);
        Task DeleteAsync(Guid recipeId, Guid customerId);
    }
}
