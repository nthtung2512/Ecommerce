using MealMate.DAL.Entities.Chatbot;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories.Chatbot;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories.Chatbot
{
    internal class RecipeRepository : IRecipeRepository
    {
        private readonly MealMateDbContext _context;
        public RecipeRepository(MealMateDbContext context)
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

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            return await _context.Recipes.Include(r => r.Ingredients).ToListAsync();
        }
    }
}
