using MealMate.DAL.Entities.ApplicationUser;

namespace MealMate.DAL.IRepositories
{
    public interface IEmployeeRepository : IRepository<StoreManager, Guid>
    {
        Task<StoreManager?> GetStoreManagerForLoginAsync(string email, string password);
    }
}
