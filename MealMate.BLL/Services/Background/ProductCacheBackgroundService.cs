using MealMate.BLL.Dtos.Product;
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

            var ttl = TimeSpan.FromHours(5); // customize if needed

            // Cache products by store
            var stores = await storeRepository.GetAllStoresAsync();
            var storeIds = stores.Select(s => s.Id).ToList();
            foreach (var storeId in storeIds)
            {
                var key = $"products-store:{storeId}";
                var existing = await redisCacheService.GetDataAsync<List<ProductDto>>(key);
                if (existing != null)
                {
                    _logger.LogInformation($"Cache already exists for store {storeId}, skipping.");
                    continue;
                }

                try
                {
                    var products = await productService.GetListProductByStoreIDAsync(storeId);
                    await redisCacheService.SetDataAsync(key, products, ttl);
                    _logger.LogInformation($"Cached products for store {storeId}");
                }
                catch (EntityNotFoundException)
                {
                    _logger.LogWarning($"No products found for store {storeId}");
                }
            }

            // Cache products by category
            var categories = await productService.GetAllCategories();
            foreach (var category in categories)
            {
                var key = $"products-category:{category}";
                var existing = await redisCacheService.GetDataAsync<List<ProductDto>>(key);
                if (existing != null)
                {
                    _logger.LogInformation($"Cache already exists for category {category}, skipping.");
                    continue;
                }

                try
                {
                    var products = await productService.GetListProductByCategoryAsync(category);
                    await redisCacheService.SetDataAsync(key, products, ttl);
                    _logger.LogInformation($"Cached products for category {category}");
                }
                catch (EntityNotFoundException)
                {
                    _logger.LogWarning($"No products found for category {category}");
                }
            }

            _logger.LogInformation("Product caching completed.");
        }
    }

}
