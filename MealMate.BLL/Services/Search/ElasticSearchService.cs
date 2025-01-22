using MealMate.BLL.IServices.Search;
using MealMate.DAL.IRepositories;
using StackExchange.Redis;

namespace MealMate.BLL.Services.Search
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IDatabase _db;
        private readonly IProductRepository _productRepository;
        public ElasticSearchService(IConnectionMultiplexer redis, IProductRepository productRepository)
        {
            _db = redis.GetDatabase();
            _productRepository = productRepository;
        }

        // Index Products
        public async Task IndexProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            foreach (var product in products)
            {
                var key = $"product:{product.Id}";
                var hashEntries = new HashEntry[]
                {
                new HashEntry("name", product.PName),
                };
                await _db.HashSetAsync(key, hashEntries);
            }
        }

        // Search Products
        public async Task<IEnumerable<string>> SearchAsync(string query)
        {
            var result = await _db.ExecuteAsync("FT.SEARCH", "products_index", $"*{query}*");
            var results = (RedisResult[])result;
            if (results == null || results.Length < 2)
            {
                return [];
            }

            var productsName = new List<string>();
            // Start from 1 to skip the total count
            for (int i = 1; i < results.Length; i += 2)
            {
                // Get the array of properties for this document
                var properties = (RedisResult[])results[i + 1];
                // Iterate through properties to find the name
                for (int j = 0; j < properties.Length; j += 2)
                {
                    if (properties[j].ToString() == "name")
                    {
                        productsName.Add(properties[j + 1].ToString());
                        break;
                    }
                }
            }

            return productsName;
        }
    }
}