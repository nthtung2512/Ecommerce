using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Repositories.auth;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class CustomerRepository(MealMateDbContext context) : IdentityRepository<Customer>(context), ICustomerRepository
    {
        public async Task<List<Customer>> GetCustomersListAsync()
        {
            // Return the list directly or an empty list if there are no results
            return await _context.Customers.ToListAsync() ?? [];
        }

        public async Task<Guid> GetLastCustomerIdAsync()
        {
            return await _context.Customers.OrderByDescending(c => c.Id).Select(c => c.Id).FirstOrDefaultAsync();
        }
    }
}
