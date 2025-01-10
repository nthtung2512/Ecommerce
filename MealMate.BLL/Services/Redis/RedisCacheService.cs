using MealMate.BLL.IServices.Redis;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace MealMate.BLL.Services.Redis
{
    internal class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<T?> GetDataAsync<T>(string key)
        {
            var data = await _distributedCache.GetStringAsync(key);
            if (data == null)
            {
                return default;
            }
            return System.Text.Json.JsonSerializer.Deserialize<T>(data);
        }

        public virtual async Task SetDataAsync<T>(string key, T data, TimeSpan? ttl = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl ?? TimeSpan.FromHours(3) // Default to 3 hours
            };

            await _distributedCache.SetStringAsync(key, System.Text.Json.JsonSerializer.Serialize(data), options);
        }

        public async Task RemoveDataAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public List<string> ScanKeys(string pattern)
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var keys = new List<RedisKey>();

            foreach (var key in server.Keys(pattern: pattern, pageSize: 1000))
            {
                keys.Add(key);
            }

            return keys.Select(k => k.ToString()).ToList();
        }
    }

}
