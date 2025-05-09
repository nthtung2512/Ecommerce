using MealMate.DAL.Entities.Stores;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class AtRepository : IAtRepository
    {
        private readonly MealMateDbContext _context;
        private IQueryable<AT> Query => _context.ATs.IsNotDeleted();

        public AtRepository(MealMateDbContext context)
        {
            _context = context;
        }
        #region dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
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
            return await Query.Where(at => at.StoreID == storeId).Include(a => a.Product).ToListAsync() ?? [];
        }

        public async Task UpdateAsync(AT updateData)
        {
            _context.Entry(updateData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(AT at)
        {
            await _context.ATs.AddAsync(at);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AT>> GetAtForProductsAsync(List<Guid> productIds, Guid storeId)
        {
            return await _context.ATs
                .Where(s => productIds.Contains(s.ProductID) && s.StoreID == storeId)
                .ToListAsync();
        }

        public async Task<List<Store>> GetStoresByProductIdAsync(Guid productId)
        {
            return await Query
                .Where(at => at.ProductID == productId)
                .Select(at => at.Store)
                .Distinct()
                .ToListAsync();
        }

        public async Task DeleteAsync(AT deleteData)
        {
            deleteData.IsDeleted = true;
            _context.Entry(deleteData).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
