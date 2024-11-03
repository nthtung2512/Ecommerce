using MealMate.DAL.Entities.ApplicationUser;

namespace MealMate.DAL.IRepositories
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<List<Customer>> GetCustomersListAsync();
        Task<Guid> GetLastCustomerIdAsync();
        Task<Customer?> GetCustomerForLoginAsync(string email, string password);
    }
}
