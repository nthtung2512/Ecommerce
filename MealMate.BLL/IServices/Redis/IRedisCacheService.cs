namespace MealMate.BLL.IServices.Redis
{
    public interface IRedisCacheService
    {
        Task<T?> GetDataAsync<T>(string key);
        Task SetDataAsync<T>(string key, T data);
        Task RemoveDataAsync(string key);
        List<string> ScanKeys(string pattern);
    }
}
