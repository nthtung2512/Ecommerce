using MealMate.DAL.Entities.Transactions;

namespace MealMate.DAL.IRepositories
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        Task<List<Product>> GetListProductByStoreIDAsync(Guid storeId);
        Task<List<Product>> GetListProductByCategoryAsync(string category);
        Task<List<Product>> GetListProductHavePromotionAsync();
        Task<List<TempTop5Product>> GetTempTop5ProductsAsync(int year);
        Task<Product?> GetProductByNameAsync(string productName);
        Task DeleteProductAtStoreAsync(Guid productId, Guid storeId);
    }
}
