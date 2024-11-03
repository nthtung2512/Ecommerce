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
            var productsFromPromoteProducts = await _context.PromoteProducts.Where(p => p.Product.IsDeleted == false).Select(p => p.Product).ToListAsync();

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
    }
}
