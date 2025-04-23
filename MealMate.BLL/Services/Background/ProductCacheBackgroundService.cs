using MealMate.BLL.IServices;
using MealMate.BLL.IServices.Redis;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MealMate.BLL.Services.Background
{
    public class ProductCacheBackgroundService : BackgroundService
    {
        private readonly ILogger<ProductCacheBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ProductCacheBackgroundService(
            ILogger<ProductCacheBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var redisCacheService = scope.ServiceProvider.GetRequiredService<IRedisCacheService>();
            var productService = scope.ServiceProvider.GetRequiredService<IProductAppService>();
            var storeRepository = scope.ServiceProvider.GetRequiredService<IStoreRepository>();

            var ttl = TimeSpan.FromHours(5);

            try
            {
                await CacheProductsByStore(redisCacheService, productService, storeRepository, ttl);
                await CacheProductsByCategory(redisCacheService, productService, ttl);
                await CachePromotionalProducts(redisCacheService, productService, ttl);
                await CacheTop5Products(redisCacheService, productService, ttl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during product caching operation.");
            }

            _logger.LogInformation("Product caching completed.");
        }

        private async Task CacheProductsByStore(IRedisCacheService redis, IProductAppService productService, IStoreRepository storeRepo, TimeSpan ttl)
        {
            var stores = await storeRepo.GetAllStoresAsync();
            foreach (var store in stores)
            {
                var key = $"products-store:{store.Id}";
                await redis.RemoveDataAsync(key);

                try
                {
                    var products = await productService.GetListProductByStoreIDAsync(store.Id);
                    await redis.SetDataAsync(key, products, ttl);
                    _logger.LogInformation("Cached products for store {StoreId}", store.Id);
                }
                catch (EntityNotFoundException)
                {
                    _logger.LogWarning("No products found for store {StoreId}", store.Id);
                }
            }
        }

        private async Task CacheProductsByCategory(IRedisCacheService redis, IProductAppService productService, TimeSpan ttl)
        {
            var categories = await productService.GetAllCategories();
            foreach (var category in categories)
            {
                var key = $"products-category:{category}";
                await redis.RemoveDataAsync(key);

                try
                {
                    var products = await productService.GetListProductByCategoryAsync(category);
                    await redis.SetDataAsync(key, products, ttl);
                    _logger.LogInformation("Cached products for category {Category}", category);
                }
                catch (EntityNotFoundException)
                {
                    _logger.LogWarning("No products found for category {Category}", category);
                }
            }
        }

        private async Task CachePromotionalProducts(IRedisCacheService redis, IProductAppService productService, TimeSpan ttl)
        {
            const string key = "products-promotion";
            await redis.RemoveDataAsync(key);

            try
            {
                var products = await productService.GetListProductHavePromotionAsync();
                await redis.SetDataAsync(key, products, ttl);
                _logger.LogInformation("Cached products with promotions");
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning("No products found with promotions");
            }
        }

        private async Task CacheTop5Products(IRedisCacheService redis, IProductAppService productService, TimeSpan ttl)
        {
            const string key = "products-top5";
            await redis.RemoveDataAsync(key);

            try
            {
                var year = DateTime.UtcNow.Year;
                var top5Products = await productService.GetTempTop5ProductsAsync(year);
                await redis.SetDataAsync(key, top5Products, ttl);
                _logger.LogInformation("Cached top 5 products");
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning("No top 5 products found");
            }
        }
    }

}
