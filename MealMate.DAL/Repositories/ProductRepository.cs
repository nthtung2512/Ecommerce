using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class ProductRepository(MealMateDbContext context) : Repository<Product, Guid>(context), IProductRepository
    {
        public async Task<List<Product>> GetListProductByCategoryAsync(string category)
        {
            return await Query.Where(p => p.Category == category).ToListAsync();
        }

        public async Task<List<Product>> GetListProductByStoreIDAsync(Guid storeId)
        {
            return await _context.ATs.IsNotDeleted().Where(a => a.StoreID == storeId).Select(a => a.Product).ToListAsync();
        }

        public async Task<List<Product>> GetListProductHavePromotionAsync()
        {
            var productsFromPromoteProducts = await _context.PromoteProducts.Where(p => p.ProductPromotion.StartDay <= DateTime.UtcNow.AddHours(7) && p.ProductPromotion.EndDay >= DateTime.UtcNow.AddHours(7)).Select(p => p.Product).ToListAsync();

            var productsFromPromoteCategory = await _context.PromoteCategories.Where(p => p.Product.IsDeleted == false).Select(p => p.Product).ToListAsync();

            return productsFromPromoteProducts.Concat(productsFromPromoteCategory).ToList();
        }

        public Task<Product?> GetProductByNameAsync(string productName)
        {
            return Query.FirstOrDefaultAsync(p => p.PName == productName);
        }

        public async Task<List<TempTop5Product>> GetTempTop5ProductsAsync(int year)
        {
            var topProducts = await _context.Includes
                .Where(i => i.Transaction.DateAndTime.Year == year && !i.Product.IsDeleted)
                .GroupBy(i => new { i.ProductID, i.Product.PName })
                .Select(g => new TempTop5Product
                {
                    ProductID = g.Key.ProductID,
                    Name = g.Key.PName,
                    Revenue = g.Sum(i => i.SubTotal)
                })
                .OrderByDescending(p => p.Revenue)
                .Take(5)
                .ToListAsync();

            return topProducts;
        }

        public async Task<List<Product>> GetProductsByListNameAsync(List<string> productNames)
        {
            return await Query.Where(p => productNames.Contains(p.PName)).ToListAsync();
        }

        public override async Task DeleteAsync(Product entity)
        {
            entity.IsDeleted = true;
            _context.Entry(entity).State = EntityState.Modified;

            var ATs = await _context.ATs.Where(a => a.ProductID == entity.Id).ToListAsync();

            foreach (var at in ATs)
            {
                at.IsDeleted = true;
                _context.Entry(at).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAtStoreAsync(Guid productId, Guid storeId)
        {
            var at = await _context.ATs.FirstOrDefaultAsync(a => a.ProductID == productId && a.StoreID == storeId);
            if (at != null)
            {
                at.IsDeleted = true;
                _context.Entry(at).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await Query.ToListAsync();
        }
    }
}
