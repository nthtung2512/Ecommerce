using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class ProductPromotionRepository : IProductPromotionRepository
    {
        private readonly MealMateDbContext _context;

        public ProductPromotionRepository(MealMateDbContext context)
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

        public async Task CreateAsync(ProductPromotion newPromotion)
        {
            await _context.ProductPromotions.AddAsync(newPromotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductPromotion productPromotion)
        {
            productPromotion.IsDeleted = true;
            _context.Entry(productPromotion).State = EntityState.Modified;

            foreach (var promoteProduct in productPromotion.PromoteProducts)
            {
                promoteProduct.IsDeleted = true;
                _context.Entry(promoteProduct).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductPromotion>> GetAllProductPromotionsAsync()
        {
            return await _context.ProductPromotions
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<ProductPromotion?> GetProductPromotionByIdAsync(Guid id)
        {
            return await _context.ProductPromotions
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<List<ProductPromotion>> GetPromotionByProductIdAsync(Guid productId)
        {
            var promotion = await _context.PromoteProducts
                        .Include(p => p.ProductPromotion)
                        .Where(p => p.ProductId == productId && !p.IsDeleted)
                        .Select(p => p.ProductPromotion)
                        .ToListAsync();
            return promotion;
        }
    }
}
