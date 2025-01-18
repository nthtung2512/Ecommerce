using MealMate.BLL.Dtos.Cart;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.Dtos.Stores;

namespace MealMate.BLL.IServices.Redis
{
    public interface IReserveCartCacheService
    {
        Task CheckoutCartAsync(CartReturnDto cart, List<CustomerPromotionDto> customerPromotions);
        Task RemoveReserveCartAsync(Guid customerId);
        Task<CheckoutRequestDto> GetCheckoutDataAsync(Guid customerId);
        Task HandleExpiredCartAsync(string customerId);
        Task<int?> GetCartTimerAsync(Guid customerId);
        Task<bool> CheckIfCustomerCheckoutAsync(Guid customerId);
        Task<int> GetQuantityFromCache(Guid productId, Guid storeId);
        Task ReduceStockOnCheckoutAsync(CartReturnDto cart);
        Task AddStockOnNoPurchaseAsync(CartReturnDto cart);
        Task<ATDto> GetAtByProductIDAndStoreIDAsync(Guid productId, Guid storeId);
        Task<List<ATDto>> GetAtByProductIDAsync(Guid productId);
        Task<List<ATDto>> GetAtByStoreIdAsync(Guid storeId);
    }
}
