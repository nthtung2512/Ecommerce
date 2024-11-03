using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Transactions;

namespace MealMate.DAL.IRepositories
{
    public interface ITransactionRepository : IRepository<Bill, Guid>
    {
        Task<List<Bill>> GetBillListAsync(Guid customerId);
        Task<List<Include>> GetAllItemsByBillIdAsync(Guid transactionId);
        Task<List<Product>> GetListProductByPromotionIDAsync(Guid promotionId);
    }
}
