using MealMate.Base;
using MealMate.BLL.Dtos.Chatbot;
using MealMate.BLL.IServices.Chatbot;
using MealMate.DAL.Entities.Chatbot;
using MealMate.DAL.IRepositories.Chatbot;
using MealMate.DAL.IRepositories.UnitOfWork;
using MealMate.DAL.Utils.Exceptions;

namespace MealMate.BLL.Services.Chatbot
{
    internal class RecipeRatingAppService : IRecipeRatingAppService
    {
        private readonly IRecipeRatingRepository _recipeRatingRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeRatingAppService(IRecipeRatingRepository recipeRatingRepository, IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
        {
            _recipeRatingRepository = recipeRatingRepository;
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
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

        private async Task<Recipe> UpdateAverageRatingAsync(Guid recipeId)
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId)
                         ?? throw new EntityNotFoundException("Recipe not found");

            var ratings = await _recipeRatingRepository.GetAllRatingByRecipeIdAsync(recipeId);
            if (ratings.Count == 0)
                return recipe;

            var average = Math.Round(ratings.Average(r => r.Rating), 1);

            recipe.AverageRating = average;

            return await _recipeRepository.UpdateAsync(recipe);
        }

        private static RecipeReturnDto MapRecipeDto(Recipe recipe)
        {
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

        public async Task<RecipeReturnDto> CreateRecipeRatingAsync(RecipeRatingCreateDto entity)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _recipeRatingRepository.CreateAsync(new RecipeRating
                {
                    RecipeId = entity.RecipeId,
                    CustomerId = entity.CustomerId,
                    Rating = entity.Rating
                });

                var updatedRecipe = await UpdateAverageRatingAsync(entity.RecipeId);

                await _unitOfWork.CommitTransactionAsync();
                return MapRecipeDto(updatedRecipe); // Return the updated recipe with the new rating
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<RecipeReturnDto> DeleteRecipeRatingAsync(Guid recipeId, Guid customerId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var recipeRating = await _recipeRatingRepository.GetRecipeRatingAsync(recipeId, customerId)
                                        ?? throw new EntityNotFoundException("No rating found for this recipe");

                await _recipeRatingRepository.DeleteAsync(recipeRating);
                var updatedRecipe = await UpdateAverageRatingAsync(recipeId);

                await _unitOfWork.CommitTransactionAsync();

                return MapRecipeDto(updatedRecipe);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<RecipeReturnDto> UpdateRecipeRatingAsync(RecipeRatingCreateDto entity)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var recipeRating = await _recipeRatingRepository.GetRecipeRatingAsync(entity.RecipeId, entity.CustomerId)
                                        ?? throw new EntityNotFoundException("No rating found for this recipe");

                recipeRating.Rating = entity.Rating;
                recipeRating.Timestamp = Constants.Now;

                await _recipeRatingRepository.UpdateAsync(recipeRating);
                var updatedRecipe = await UpdateAverageRatingAsync(entity.RecipeId);

                await _unitOfWork.CommitTransactionAsync();

                return MapRecipeDto(updatedRecipe);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
