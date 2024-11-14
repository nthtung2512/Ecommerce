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

        public async Task<List<Bill>> GetAllBillAsync()
        {
            return await _context.Bills.Include(b => b.Includes).ToListAsync();
        }

        public async Task<List<Bill>> GetBillListByStoreIdAsync(Guid storeId, DeliveryStatus status)
        {
            return await _context.Bills.Include(b => b.Includes).ThenInclude(i => i.Product).Where(b => b.StoreID == storeId && !b.IsDeleted && b.DeliveryStatus == status).ToListAsync();
        }
    }
}
