using MealMate.BLL.Dtos.Bills;

namespace MealMate.BLL.IServices.Redis
{
    public interface IReserveCartItemCacheService
    {
        Task<CartItemReturnDto?> GetCartItemCache(Guid productId, Guid storeId);
        Task AddCartItemToRedis(CartItemReturnDto item);
        Task RemoveCartItemFromRedis(Guid productId, Guid storeId);
    }
}
