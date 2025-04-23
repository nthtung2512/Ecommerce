using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.Repositories;
using MealMate.DAL.Utils.Enum;

namespace MealMate.DAL.IRepositories
{
    public interface ITransactionRepository : IRepository<Bill, Guid>
    {
        Task<List<Bill>> GetAllBillAsync();
        Task<List<Bill>> GetBillListAsync(Guid customerId);
        Task<List<Bill>> GetDetailBillListAsync(Guid customerId);
        Task<List<Bill>> GetBillListByStatusAsync(DeliveryStatus status);
        Task<List<Bill>> GetBillListByStoreIdAsync(Guid storeId);
        Task<BillStatusStatisticsDto> GetBillStatusStatisticsByStoreIdPrevAsync(Guid storeId);
        Task<List<Bill>> GetBillListByStoreIdAndStatusAsync(Guid storeId, DeliveryStatus status);
        Task<List<Include>> GetAllItemsByBillIdAsync(Guid transactionId);
        Task<List<Product>> GetListProductByPromotionIDAsync(Guid promotionId);
    }
}
