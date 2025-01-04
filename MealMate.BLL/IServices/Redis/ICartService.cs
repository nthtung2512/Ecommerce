using MealMate.BLL.Dtos.Bills;
using MealMate.DAL.Entities.Transactions;

namespace MealMate.BLL.IServices.Redis
{
    public interface ICartService
    {
        Task<List<CartItemReturnDto>> GetCartAsync(Guid customerId);
        Task<List<CartItemReturnDto>> AddProductToCartAsync(Guid customerId, CartItem item);
        Task<List<CartItemReturnDto>> UpdateProductQuantityAsync(Guid customerId, Guid productId, int quantity);
        Task<List<CartItemReturnDto>> RemoveProductFromCartAsync(Guid customerId, Guid productId);
        Task<List<CartItemReturnDto>> ClearCartAsync(Guid customerId);
        Task<List<CartItemReturnDto>> RevalidateCartWithCustomerIdAsync(Guid customerId);
        Task<List<CartItemReturnDto>> GetCartWithRevalidationAsync(Guid customerId);
        Task RevalidateCartsWithProductIdsAsync(List<Guid> productIds);
    }
}
