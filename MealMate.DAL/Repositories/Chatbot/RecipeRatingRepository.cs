using MealMate.DAL.Entities.Chatbot;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories.Chatbot;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories.Chatbot
{
    internal class RecipeRatingRepository : IRecipeRatingRepository
    {
        private readonly MealMateDbContext _context;
        public RecipeRatingRepository(MealMateDbContext context)
        {
            _context = context;
        }

        #region dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        public async Task CreateAsync(RecipeRating entity)
        {
            await _context.RecipeRatings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RecipeRating recipeRating)
        {
            _context.RecipeRatings.Remove(recipeRating);
            await _context.SaveChangesAsync();
        }

        public async Task<RecipeRating?> GetRecipeRatingAsync(Guid recipeId, Guid customerId)
        {
            return await _context.RecipeRatings.FindAsync(recipeId, customerId);
        }

        public async Task<List<RecipeRating>> GetAllRatingByRecipeIdAsync(Guid recipeId)
        {
            return await _context.RecipeRatings
                .Where(r => r.RecipeId == recipeId)
                .ToListAsync();
        }

        public async Task UpdateAsync(RecipeRating entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
