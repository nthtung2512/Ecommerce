using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.IRepositories.auth;

namespace MealMate.DAL.IRepositories
{
    public interface IEmployeeRepository : IIdentityRepository<StoreManager>
    {
        Task<List<StoreManager>> GetStoreManagerListAsync();
        Task<StoreManager?> GetEmployeeByStoreIdAsync(Guid storeid);
    }
}
