using StackExchange.Redis;

namespace MealMate.BLL.IServices.Redis
{
    public interface IRedisCacheService
    {
        IDatabase GetDatabase();
        Task<T?> GetDataAsync<T>(string key);
        Task SetDataAsync<T>(string key, T data, TimeSpan? ttl = null);
        Task RemoveDataAsync(string key);
        List<string> ScanKeys(string pattern);
    }
}
