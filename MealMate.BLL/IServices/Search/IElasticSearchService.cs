namespace MealMate.BLL.IServices.Search
{
    public interface IElasticSearchService
    {
        Task IndexProductsAsync();
        Task<IEnumerable<string>> SearchAsync(string query);
    }
}
