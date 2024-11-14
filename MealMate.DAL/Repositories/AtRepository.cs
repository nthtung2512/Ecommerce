using MealMate.DAL.Entities.Stores;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class AtRepository(MealMateDbContext context) : IAtRepository
    {
        private IQueryable<AT> Query => context.ATs.IsNotDeleted();
        public async Task<AT?> GetAtByProductIDAndStoreIDAsync(Guid productID, Guid storeID)
        {
            return await Query.Include(a => a.Product).FirstOrDefaultAsync(at => at.ProductID == productID && at.StoreID == storeID);
        }

        public async Task<List<AT>> GetAtByProductIDAsync(Guid productID)
        {
            return await Query.Where(at => at.ProductID == productID).ToListAsync() ?? [];
        }

        public async Task<List<AT>> GetAtByStoreIdAsync(Guid storeId)
        {
            return await Query.Where(at => at.StoreID == storeId).ToListAsync() ?? [];
        }

        public async Task UpdateAsync(AT updateData)
        {
            context.Entry(updateData).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task CreateAsync(AT at)
        {
            await context.ATs.AddAsync(at);
            await context.SaveChangesAsync();
        }
    }
}
