using MealMate.BLL.Dtos.Chatbot;

namespace MealMate.BLL.IServices.Chatbot
{
    public interface IRecipeRatingAppService
    {
        Task<RecipeRatingReturnDto> GetRecipeRatingAsync(Guid recipeId, Guid customerId);
        Task<RecipeReturnDto> CreateRecipeRatingAsync(RecipeRatingCreateDto entity);
        Task<RecipeReturnDto> UpdateRecipeRatingAsync(RecipeRatingCreateDto entity);
        Task<RecipeReturnDto> DeleteRecipeRatingAsync(Guid recipeId, Guid customerId);
    }
}
