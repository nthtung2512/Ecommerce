using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.Utils.Enum;

namespace MealMate.DAL.IRepositories
{
    public interface ITransactionRepository : IRepository<Bill, Guid>
    {
        Task<List<Bill>> GetAllBillAsync();
        Task<List<Bill>> GetBillListAsync(Guid customerId);
        Task<List<Bill>> GetBillListByStoreIdAsync(Guid storeId, DeliveryStatus status);
        Task<List<Include>> GetAllItemsByBillIdAsync(Guid transactionId);
        Task<List<Product>> GetListProductByPromotionIDAsync(Guid promotionId);
    }
}
