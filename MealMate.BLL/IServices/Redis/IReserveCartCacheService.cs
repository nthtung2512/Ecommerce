using MealMate.BLL.Dtos.Cart;
using MealMate.BLL.Dtos.Stores;

namespace MealMate.BLL.IServices.Redis
{
    public interface IReserveCartCacheService
    {
        Task CheckoutCartAsync(CartReturnDto cart);
        Task RemoveReserveCartAsync(Guid customerId);
        Task HandleExpiredCartAsync(string customerId);
        Task<bool> CheckIfCustomerCheckoutAsync(Guid customerId);
        Task<int> GetQuantityFromCache(Guid productId, Guid storeId);
        Task<ATDto> GetAtByProductIDAndStoreIDAsync(Guid productId, Guid storeId);
        Task<List<ATDto>> GetAtByProductIDAsync(Guid productId);
        Task<List<ATDto>> GetAtByStoreIdAsync(Guid storeId);
    }
}
