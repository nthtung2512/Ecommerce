using MealMate.BLL.Dtos.Product;

namespace MealMate.BLL.IServices.Redis
{
    public interface IProductCacheService
    {
        Task InvalidateProductCacheAsync(ProductDto productDto);
        Task InvalidProductCategoryCacheAsync(string category);
        Task InvalidProductAtStoreCacheAsync(Guid storeId);
    }
}
