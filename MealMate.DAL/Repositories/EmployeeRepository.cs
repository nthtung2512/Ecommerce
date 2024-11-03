using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class EmployeeRepository(MealMateDbContext context) : Repository<StoreManager, Guid>(context), IEmployeeRepository
    {
        public override async Task<StoreManager?> GetAsync(Guid id)
        {
            return await _context.StoreManagers
                .Include(e => e.EPhones)
                .FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public async Task<StoreManager?> GetStoreManagerForLoginAsync(string email, string password)
        {
            return await _context.StoreManagers
                .Include(e => e.EPhones)
                .FirstOrDefaultAsync(e => e.Email.Equals(email));
        }

        public override async Task DeleteAsync(StoreManager storeManager)
        {
            storeManager.IsDeleted = true;
            _context.Entry(storeManager).State = EntityState.Modified;
            storeManager.EPhones.Clear();
            await _context.SaveChangesAsync();
        }
    }
}
