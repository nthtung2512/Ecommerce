using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class CategoryPromotionRepository : ICategoryPromotionRepository
    {
        private readonly MealMateDbContext _context;

        public CategoryPromotionRepository(MealMateDbContext context)
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

        public async Task<List<ProductCategoryPromotion>> GetCategoryPromotionByCategoryAsync(string category)
        {
            // Query the database, excluding deleted entries
            var promotions = await _context.PromoteCategories
                .Include(p => p.ProductCategoryPromotion)
               .Where(p => p.ProductCategoryPromotion.Category == category && !p.IsDeleted)
               .Select(p => p.ProductCategoryPromotion)
               .ToListAsync();

            return promotions;
        }

        public async Task<List<ProductCategoryPromotion>> GetAllCategoryPromotionsAsync()
        {
            return await _context.ProductCategoryPromotions.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<ProductCategoryPromotion?> GetCategoryPromotionByIdAsync(Guid id)
        {
            return await _context.ProductCategoryPromotions.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task CreateAsync(ProductCategoryPromotion newPromotion)
        {
            await _context.ProductCategoryPromotions.AddAsync(newPromotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductCategoryPromotion productPromotion)
        {
            productPromotion.IsDeleted = true;
            _context.Entry(productPromotion).State = EntityState.Modified;

            foreach (var promoteProduct in productPromotion.PromoteProductCategories)
            {
                promoteProduct.IsDeleted = true;
                _context.Entry(promoteProduct).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }
    }
}
