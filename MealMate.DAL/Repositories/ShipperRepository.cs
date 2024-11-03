using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class ShipperRepository(MealMateDbContext context) : IShipperRepository
    {
        protected readonly MealMateDbContext _context = context;
        protected IQueryable<Shipper> Query => _context.Shippers.IsNotDeleted().Include(s => s.ShipperPhoneNos).Include(s1 => s1.AreaShips);

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


        public async Task CreateAsync(Shipper entity)
        {
            await _context.Shippers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Shipper entity)
        {
            entity.IsDeleted = true;
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task<Shipper?> GetAsync(Guid id)
        {
            return await Query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public async Task<List<Shipper>> GetFreeShipperByAreaAsync(string area)
        {
            return await Query.Where(s => s.AreaShips.Any(a => a.Area == area)).ToListAsync();
        }

        public async Task UpdateAsync(Shipper shipper)
        {
            _context.Entry(shipper).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Shipper?> GetPhoneNoAsync(string phoneNo)
        {
            return await Query.FirstOrDefaultAsync(s => s.ShipperPhoneNos.Any(p => p.PhoneNo == phoneNo));
        }
    }
}
