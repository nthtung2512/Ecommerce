using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.IServices.Redis;

namespace MealMate.BLL.Services.Redis
{
    internal class ReserveCartItemCacheService : IReserveCartItemCacheService
    {
        private readonly IRedisCacheService _redisCacheService;

        public ReserveCartItemCacheService(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        public async Task<CartItemReturnDto?> GetCartItemCache(Guid productId, Guid storeId)
        {
            var key = $"CartItemReserve:{productId}-{storeId}";
            return await _redisCacheService.GetDataAsync<CartItemReturnDto>(key);
        }

        public async Task AddCartItemToRedis(CartItemReturnDto item)
        {
            var key = $"CartItemReserve:{item.ProductID}-{item.StoreID}";

            var cartItem = await GetCartItemCache(item.ProductID, item.StoreID);
            if (cartItem != null)
            {
                item.Quantity += cartItem.Quantity;
            }
            await _redisCacheService.SetDataAsync(key, item, TimeSpan.FromMinutes(15));
        }

        public async Task RemoveCartItemFromRedis(Guid productId, Guid storeId)
        {
            var key = $"CartItemReserve:{productId}-{storeId}";
            await _redisCacheService.RemoveDataAsync(key);
        }
    }
}
