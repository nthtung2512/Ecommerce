using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Enum;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class TransactionRepository : Repository<Bill, Guid>, ITransactionRepository
    {
        public TransactionRepository(MealMateDbContext context) : base(context)
        {
        }

        public async Task<List<Include>> GetAllItemsByBillIdAsync(Guid transactionId)
        {
            // Await the result of FirstOrDefaultAsync to get the Bill
            var bill = await Query
                .Include(b => b.Includes)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(b => b.Id == transactionId);

            // Check if the bill is not null before proceeding
            if (bill != null)
            {
                // Use SelectMany on the Includes of the bill to get the products
                return [.. bill.Includes];
            }

            // Return an empty list if no bill is found
            return [];
        }

        public override async Task<Bill?> GetAsync(Guid id)
        {
            return await _context.Bills.Include(b => b.Includes).ThenInclude(p => p.Product).FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        /*public override async Task DeleteAsync(Bill bill)
        {
            bill.IsDeleted = true;
            _context.Entry(bill).State = EntityState.Modified;
            foreach (var include in bill.Includes)
            {
                include.IsDeleted = true;
                _context.Entry(include).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }*/
        public async Task<List<Product>> GetListProductByPromotionIDAsync(Guid promotionId)
        {
            // Fetch products from PromoteProducts that match the promotionId and are not deleted
            var productsFromPromoteProducts
                = await _context.PromoteProducts
                        .Where(p => p.PromotionId == promotionId && p.Product.IsDeleted == false)
                        .Select(p => p.Product)
                        .ToListAsync();

            // Fetch products from PromoteCategory that match the promotionId and are not deleted
            var productsFromPromoteCategory
                = await _context.PromoteCategories
                        .Where(p => p.PromotionId == promotionId && p.Product.IsDeleted == false)
                        .Select(p => p.Product)
                        .ToListAsync();

            // Combine the two lists and remove duplicates, if any
            var allProducts = productsFromPromoteProducts.Concat(productsFromPromoteCategory)
                                                         .Distinct()
                                                         .ToList();

            return allProducts;
        }


        public async Task<List<Bill>> GetBillListAsync(Guid customerId)
        {
            return await _context.Bills.Where(b => b.CustomerID == customerId).ToListAsync();
        }

        public async Task<List<Bill>> GetDetailBillListAsync(Guid customerId)
        {
            return await _context.Bills.Include(b => b.Includes).ThenInclude(i => i.Product).Where(b => b.CustomerID == customerId && !b.IsDeleted).ToListAsync();
        }

        public async Task<List<Bill>> GetAllBillAsync()
        {
            return await _context.Bills.Include(b => b.Includes).ToListAsync();
        }

        public async Task<List<Bill>> GetBillListByStoreIdAndStatusAsync(Guid storeId, DeliveryStatus status)
        {
            return await _context.Bills.Include(b => b.Includes).ThenInclude(i => i.Product).Where(b => b.StoreID == storeId && !b.IsDeleted && b.DeliveryStatus == status).ToListAsync();
        }

        public async Task<List<Bill>> GetBillListByStatusAsync(DeliveryStatus status)
        {
            return await _context.Bills.Include(b => b.Includes).ThenInclude(i => i.Product).Where(b => b.DeliveryStatus == status && !b.IsDeleted).ToListAsync();
        }

        public async Task<List<Bill>> GetBillListByStoreIdAsync(Guid storeId)
        {
            return await _context.Bills.Include(b => b.Includes).ThenInclude(i => i.Product).Where(b => b.StoreID == storeId && !b.IsDeleted).ToListAsync();
        }
        public async Task<BillStatusStatisticsDto> GetBillStatusStatisticsByStoreIdPrevAsync(Guid storeId)
        {
            var utcNow = DateTime.UtcNow;
            var utcPlus7Now = utcNow.AddHours(7);

            // Get the start of this week (Monday)
            var startOfThisWeekPlus7 = utcPlus7Now.Date.AddDays(-(int)utcPlus7Now.DayOfWeek + (int)DayOfWeek.Monday);

            // Get the start and end of last week in UTC+7
            var startOfLastWeekPlus7 = startOfThisWeekPlus7.AddDays(-7);
            var endOfLastWeekPlus7 = startOfThisWeekPlus7;

            // Convert to UTC
            var startOfLastWeekUtc = startOfLastWeekPlus7.AddHours(-7);
            var endOfLastWeekUtc = endOfLastWeekPlus7.AddHours(-7);


            var bills = await _context.Bills
                .Where(b =>
                    b.StoreID == storeId &&
                    b.DateAndTime >= startOfLastWeekUtc &&
                    b.DateAndTime < endOfLastWeekUtc &&
                    !b.IsDeleted)
                .ToListAsync();

            var totalBills = bills.Count;
            var deliveredCount = bills.Count(b => b.DeliveryStatus == DeliveryStatus.Delivered);
            var cancelledCount = bills.Count(b => b.DeliveryStatus == DeliveryStatus.Cancelled);
            var ghostCount = bills.Count(b => b.DeliveryStatus == DeliveryStatus.Ghost);

            return new BillStatusStatisticsDto
            {
                TotalBills = totalBills,
                DeliveredCount = deliveredCount,
                CancelledCount = cancelledCount,
                GhostCount = ghostCount
            };
        }


    }

    public class BillStatusStatisticsDto
    {
        public int TotalBills { get; set; }
        public int DeliveredCount { get; set; }
        public int CancelledCount { get; set; }
        public int GhostCount { get; set; }
        public double DeliveredRate => TotalBills == 0 ? 0 : (double)DeliveredCount / TotalBills;
        public double CancelledRate => TotalBills == 0 ? 0 : (double)CancelledCount / TotalBills;
        public double GhostRate => TotalBills == 0 ? 0 : (double)GhostCount / TotalBills;
    }
}
