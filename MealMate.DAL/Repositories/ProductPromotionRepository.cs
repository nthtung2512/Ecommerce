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
            _context.Remove(productPromotion);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductPromotion>> GetAllProductPromotionsAsync()
        {
            return await _context.ProductPromotions
                .Include(p => p.PromoteProducts)
                .ThenInclude(pp => pp.Product)
                .Where(p => p.StartDay <= DateTime.UtcNow.AddHours(7) && p.EndDay >= DateTime.UtcNow.AddHours(7))
                .ToListAsync();
        }

        /*public async Task<ProductPromotion?> GetProductPromotionByIdAsync(Guid id)
        {
            return await _context.ProductPromotions
                .FirstOrDefaultAsync(p => p.Id == id);
        }*/

        public async Task<List<ProductPromotion>> GetPromotionByProductIdAsync(Guid productId)
        {
            var promotion = await _context.PromoteProducts
                        .Include(p => p.ProductPromotion)
                        .Include(p => p.Product)
                        .Where(p => p.ProductId == productId
                           && p.ProductPromotion.StartDay <= DateTime.UtcNow.AddHours(7)
                           && p.ProductPromotion.EndDay >= DateTime.UtcNow.AddHours(7))
                        .Select(p => p.ProductPromotion)
                        .ToListAsync();
            return promotion;
        }

        /*        public async Task<List<PromoteProduct>> GetPromotionInfoByProductIdAsync(Guid productId)
                {
                    var promotion = await _context.PromoteProducts
                                .Include(p => p.ProductPromotion)
                                .Include(p => p.Product)
                                .Where(p => p.ProductId == productId)
                                .ToListAsync();
                    return promotion;
                }*/

        public async Task<List<ProductPromotion>> GetExpiredPromotionsAsync()
        {
            return await _context.ProductPromotions
                .Include(p => p.PromoteProducts)
                .Where(p => p.EndDay < DateTime.UtcNow.AddHours(7))
                .ToListAsync();
        }
    }
}
