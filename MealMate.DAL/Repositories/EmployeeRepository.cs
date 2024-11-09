using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Repositories.auth;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class EmployeeRepository(MealMateDbContext context) : IdentityRepository<StoreManager>(context), IEmployeeRepository
    {
        public async Task<StoreManager?> GetEmployeeByStoreIdAsync(Guid storeid)
        {
            var employee = await Query.FirstOrDefaultAsync(e => e.StoreId == storeid);
            return employee;
        }

        public Task<List<StoreManager>> GetStoreManagerListAsync()
        {
            return Query.ToListAsync();
        }
    }
}
