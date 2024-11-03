using MealMate.DAL.Entities.Stores;

namespace MealMate.DAL.IRepositories
{
    public interface IStoreRepository : IRepository<Store, Guid>
    {
        Task<List<Store>> GetAllStoresAsync();
    }
}
