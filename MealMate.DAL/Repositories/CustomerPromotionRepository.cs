using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class CustomerPromotionRepository : ICustomerPromotionRepository
    {
        private readonly MealMateDbContext _context;

        public CustomerPromotionRepository(MealMateDbContext context)
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

        public async Task CreateAsync(CustomerPromotion customerPromotion)
        {
            await _context.CustomerPromotions.AddAsync(customerPromotion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CustomerPromotion customerPromotion)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CustomerPromotion customerPromotion)
        {
            _context.Remove(customerPromotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePromoteCustomerAsync(PromoteCustomer promoteCustomer)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<CustomerPromotion?> GetCustomerPromotionByIdAsync(Guid promotionid)
        {
            return await _context.CustomerPromotions.Include(p => p.PromoteCustomers).FirstOrDefaultAsync(p => p.Id == promotionid && p.StartDay <= DateTime.UtcNow.AddHours(7) && p.EndDay >= DateTime.UtcNow.AddHours(7));
        }

        public async Task<PromoteCustomer?> GetCustomerPromotionByPCIdAsync(Guid promotionid, Guid customerid)
        {
            return await _context.PromoteCustomers.Include(p => p.CustomerPromotion).FirstOrDefaultAsync(p => p.PromotionId == promotionid && p.CustomerId == customerid && p.CustomerPromotion.StartDay <= DateTime.UtcNow.AddHours(7) && p.CustomerPromotion.EndDay >= DateTime.UtcNow.AddHours(7));
        }

        public async Task<List<CustomerPromotion>> GetDiscountByProductIdListAsync(List<Guid> productIdList)
        {
            return await _context.CustomerPromotions
                .Where(cp => productIdList.Contains(cp.ProductId))
                .Include(cp => cp.PromoteCustomers)
                .ToListAsync();
        }

        public async Task<List<CustomerPromotion>> GetExpiredCustomerPromotions()
        {
            return await _context.CustomerPromotions
                .Include(cp => cp.PromoteCustomers)
                .Where(cp => cp.EndDay < DateTime.UtcNow.AddHours(7))
                .ToListAsync();
        }

        public async Task<List<CustomerPromotion>> GetListAsync()
        {
            return await _context.CustomerPromotions
                .Include(cp => cp.PromoteCustomers)
                .ToListAsync();
        }

        public async Task<List<CustomerPromotion>> GetListByCustomerIdAsync(Guid customerId)
        {
            return await _context.CustomerPromotions
                .Include(cp => cp.PromoteCustomers)
                .Where(cp => cp.PromoteCustomers.Any(pc => pc.CustomerId == customerId))
                .ToListAsync();
        }


    }
}
