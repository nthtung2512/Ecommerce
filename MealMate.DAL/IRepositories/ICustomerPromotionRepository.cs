using MealMate.DAL.Entities.Promotion;

namespace MealMate.DAL.IRepositories
{
    public interface ICustomerPromotionRepository
    {
        Task<CustomerPromotion?> GetCustomerPromotionByIdAsync(Guid promotionid);
        Task<List<CustomerPromotion>> GetListAsync();
        Task<List<CustomerPromotion>> GetListByCustomerIdAsync(Guid customerId);
        Task<List<CustomerPromotion>> GetDiscountByProductIdListAsync(List<Guid> productIdList);
        Task<List<CustomerPromotion>> GetExpiredCustomerPromotions();
        Task<PromoteCustomer?> GetCustomerPromotionByPCIdAsync(Guid promotionid, Guid customerid);
        Task DeleteAsync(CustomerPromotion customerPromotion);
        Task DeletePromoteCustomerAsync(PromoteCustomer promoteCustomer);
        Task CreateAsync(CustomerPromotion customerPromotion);
        Task UpdateAsync(CustomerPromotion customerPromotion);
    }
}
