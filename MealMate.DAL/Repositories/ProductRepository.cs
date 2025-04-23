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
            return await Query.Where(p => p.Aisle == category).ToListAsync();
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
            return Query.FirstOrDefaultAsync(p => p.Name == productName);
        }

        public async Task<List<TempTop5Product>> GetTempTop5ProductsAsync(int year)
        {
            var topProducts = await _context.Includes
                .Where(i => i.Transaction.DateAndTime.Year == year && !i.Product.IsDeleted)
                .GroupBy(i => new { i.ProductID, i.Product.Name })
                .Select(g => new TempTop5Product
                {
                    ProductID = g.Key.ProductID,
                    Name = g.Key.Name,
                    Revenue = g.Sum(i => i.SubTotal)
                })
                .OrderByDescending(p => p.Revenue)
                .Take(5)
                .ToListAsync();

            return topProducts;
        }

        public async Task<List<Product>> GetProductsByListNameAsync(List<string> productNames)
        {
            return await Query.Where(p => productNames.Contains(p.Name)).ToListAsync();
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

        public async Task<List<string>> GetAllCategoriesAsync()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted)
                .Select(p => p.Aisle)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<TempTop5Product>> GetTempTop5StoresAsync(Guid storeID)
        {
            // Convert to UTC+7
            var utcNow = DateTime.UtcNow;
            var utcPlus7 = utcNow.AddHours(7).Date;
            var previousDay = utcPlus7.AddDays(-1);

            var previousDayUtcStart = previousDay.AddHours(-7); // Convert back to UTC
            var previousDayUtcEnd = previousDay.AddDays(1).AddHours(-7); // Next day in UTC+7

            // Query all Includes of Bills in the store on the previous day
            var topProducts = await _context.Includes
                .Where(i =>
                    i.Transaction.StoreID == storeID &&
                    i.Transaction.DateAndTime >= previousDayUtcStart &&
                    i.Transaction.DateAndTime < previousDayUtcEnd &&
                    !i.Product.IsDeleted)
                .GroupBy(i => new { i.ProductID, i.Product.Name })
                .Select(g => new TempTop5Product
                {
                    ProductID = g.Key.ProductID,
                    Name = g.Key.Name,
                    Revenue = g.Sum(i => i.SubTotal)
                })
                .OrderByDescending(p => p.Revenue)
                .Take(5)
                .ToListAsync();

            return topProducts;
        }


    }
}
