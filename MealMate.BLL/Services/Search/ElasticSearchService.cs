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

            EnsureIndexExists(); // Ensure index creation
        }

        // Ensure RedisSearch Index Exists
        private void EnsureIndexExists()
        {
            try
            {
                _db.Execute("FT.CREATE", "products_index", "ON", "HASH", "PREFIX", "1", "product:", "SCHEMA", "name", "TEXT");
            }
            catch (RedisServerException ex) when (ex.Message.Contains("Index already exists"))
            {
                // Ignore error if index already exists
            }
        }

        // Batch Index Products for better performance
        public async Task IndexProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var batch = _db.CreateBatch();

            foreach (var product in products)
            {
                var key = $"product:{product.Id}";
                var hashEntries = new HashEntry[]
                {
                    new("name", product.PName),
                };
                await batch.HashSetAsync(key, hashEntries);
            }

            batch.Execute();
        }

        // Optimized Search Products
        public async Task<IEnumerable<string>> SearchAsync(string query)
        {
            var result = await _db.ExecuteAsync("FT.SEARCH", "products_index", $"*{query}*");
            if (result.IsNull) return [];

            var results = (RedisResult[])result;
            if (results.Length < 2) return [];

            var productNames = new List<string>();

            for (int i = 1; i < results.Length; i += 2)
            {
                var properties = (RedisResult[])results[i + 1];

                for (int j = 0; j < properties.Length; j += 2)
                {
                    if (properties[j].ToString() == "name")
                    {
                        productNames.Add(properties[j + 1].ToString());
                        break;
                    }
                }
            }

            return productNames;
        }
    }
}
