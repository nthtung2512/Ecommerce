using MealMate.BLL.Dtos.Product;
using MealMate.BLL.IServices.Redis;
using MealMate.DAL.IRepositories;
using Microsoft.Extensions.Logging;

namespace MealMate.BLL.Services.Redis
{
    internal class ProductCacheService : IProductCacheService
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly IStoreRepository _storeRepository;
        private readonly ILogger<ProductCacheService> _logger;

        public ProductCacheService(IRedisCacheService redisCacheService, IStoreRepository storeRepository, ILogger<ProductCacheService> logger)
        {
            _redisCacheService = redisCacheService;
            _storeRepository = storeRepository;
            _logger = logger;
        }

        public async Task InvalidateProductCacheAsync(ProductDto productDto)
        {
            var stores = await _storeRepository.GetAllStoresAsync();
            foreach (var store in stores)
            {
                var storeKey = $"products-store:{store.Id}";
                await _redisCacheService.RemoveDataAsync(storeKey);
                _logger.LogInformation("Invalidated cache for store {StoreId}", store.Id);
            }

            var categoryKey = $"products-category:{productDto.Aisle}";
            await _redisCacheService.RemoveDataAsync(categoryKey);
            _logger.LogInformation("Invalidated cache for category {Category}", productDto.Aisle);
        }

        public async Task InvalidProductCategoryCacheAsync(string category)
        {
            var categoryKey = $"products-category:{category}";
            await _redisCacheService.RemoveDataAsync(categoryKey);
            _logger.LogInformation("Invalidated cache for category {Category}", category);
        }

        public async Task InvalidProductAtStoreCacheAsync(Guid storeId)
        {
            var storeKey = $"products-store:{storeId}";
            await _redisCacheService.RemoveDataAsync(storeKey);
            _logger.LogInformation("Invalidated cache for store {StoreId}", storeId);
        }
    }
}
