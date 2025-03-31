using MealMate.Base;
using MealMate.BLL.Dtos.Chatbot;
using MealMate.BLL.IServices.Chatbot;
using MealMate.DAL.Entities.Chatbot;
using MealMate.DAL.IRepositories.Chatbot;
using MealMate.DAL.Utils.Exceptions;

namespace MealMate.BLL.Services.Chatbot
{
    internal class RecipeRatingAppService : IRecipeRatingAppService
    {
        private readonly IRecipeRatingRepository _recipeRatingRepository;

        public RecipeRatingAppService(IRecipeRatingRepository recipeRatingRepository)
        {
            _recipeRatingRepository = recipeRatingRepository;
        }

        public async Task CreateAsync(RecipeRatingCreateDto entity)
        {
            await _recipeRatingRepository.CreateAsync(new RecipeRating
            {
                RecipeId = entity.RecipeId,
                CustomerId = entity.CustomerId,
                Rating = entity.Rating
            });
        }

        public async Task DeleteAsync(Guid recipeId, Guid customerId)
        {
            var recipeRating = await _recipeRatingRepository.GetRecipeRatingAsync(recipeId, customerId) ?? throw new EntityNotFoundException("No rating found for this recipe");

            await _recipeRatingRepository.DeleteAsync(recipeRating);
        }

        public async Task<RecipeRatingReturnDto> GetRecipeRatingAsync(Guid recipeId, Guid customerId)
        {
            var recipeRating = await _recipeRatingRepository.GetRecipeRatingAsync(recipeId, customerId) ?? throw new EntityNotFoundException("No rating found for this recipe");
            return new RecipeRatingReturnDto
            {
                RecipeId = recipeRating.RecipeId,
                CustomerId = recipeRating.CustomerId,
                Rating = recipeRating.Rating,
                Timestamp = recipeRating.Timestamp
            };
        }

        public async Task UpdateAsync(RecipeRatingCreateDto entity)
        {
            var recipeRating = await _recipeRatingRepository.GetRecipeRatingAsync(entity.RecipeId, entity.CustomerId) ?? throw new EntityNotFoundException("No rating found for this recipe");

            recipeRating.Rating = entity.Rating;
            recipeRating.Timestamp = Constants.Now;

            await _recipeRatingRepository.UpdateAsync(recipeRating);
        }
    }
}
