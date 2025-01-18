using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.Dtos.Cart;
using MealMate.BLL.Dtos.Promotion;

namespace MealMate.BLL.IServices.Redis
{
    public interface ICartService
    {
        Task<CartReturnDto?> GetCartAsync(Guid customerId);
        Task AddProductToCartAsync(Guid customerId, CartItemReturnDto item);
        /*        Task<List<CartItemReturnDto>> UpdateProductQuantityAsync(Guid customerId, Guid productId, int quantity);*/
        Task RemoveProductFromCartAsync(Guid customerId, Guid productId, Guid storeId, double discountedPrice, int quantity);
        Task ClearCartAsync(Guid customerId);
        Task<RevalidateReturnDto> RevalidateCartWithCustomerIdAsync(CartReturnDto cart);
        Task<List<CustomerPromotionDto>> SelectCustomerPromotion(Guid customerId, CustomerPromotionDto customerPromotion);
        Task<List<CustomerPromotionDto>> UnSelectCustomerPromotion(Guid customerId, List<CustomerPromotionDto> promotions);
        Task<List<CustomerPromotionDto>> GetSelectedPromotionAsync(Guid customerId);

        /*        Task RevalidateCartsWithProductIdsAsync(List<Guid> productIds);*/
    }
}
