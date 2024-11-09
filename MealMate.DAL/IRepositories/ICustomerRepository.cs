using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.IRepositories.auth;

namespace MealMate.DAL.IRepositories
{
    public interface ICustomerRepository : IIdentityRepository<Customer>
    {
        Task<List<Customer>> GetCustomersListAsync();
        Task<Guid> GetLastCustomerIdAsync();
    }
}
