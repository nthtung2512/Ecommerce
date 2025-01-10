using MealMate.BLL.Dtos.Cart;
using MealMate.DAL.Entities.Transactions;

namespace MealMate.BLL.IServices.Redis
{
    public interface ICartService
    {
        Task<CartReturnDto?> GetCartAsync(Guid customerId);
        Task AddProductToCartAsync(Guid customerId, CartItem item);
        /*        Task<List<CartItemReturnDto>> UpdateProductQuantityAsync(Guid customerId, Guid productId, int quantity);*/
        Task RemoveProductFromCartAsync(Guid customerId, Guid productId, Guid storeId, double discountedPrice, int quantity);
        Task ClearCartAsync(Guid customerId);
        Task<RevalidateReturnDto> RevalidateCartWithCustomerIdAsync(CartReturnDto cart);
        /*        Task RevalidateCartsWithProductIdsAsync(List<Guid> productIds);*/
    }
}
