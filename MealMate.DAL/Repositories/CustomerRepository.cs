using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class CustomerRepository(MealMateDbContext context) : Repository<Customer, Guid>(context), ICustomerRepository
    {
        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CEmail == email);
        }

        public async Task<Customer?> GetCustomerForLoginAsync(string email, string password)
        {
            // Fetch customer by email
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CEmail == email);
            return customer;
        }

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
