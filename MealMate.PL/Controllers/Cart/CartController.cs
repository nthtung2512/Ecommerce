using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.Dtos.Cart;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices.Redis;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers.Cart
{
    [ApiController]
    /*[Authorize(Roles = "Customer")]*/
    [Route("cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IReserveCartCacheService _reserveCartCacheService;

        public CartController(ICartService cartService, IReserveCartCacheService reserveCartCacheService)
        {
            _cartService = cartService;
            _reserveCartCacheService = reserveCartCacheService;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCart(Guid customerId)
        {
            var cart = await _cartService.GetCartAsync(customerId);
            var selectedPromotions = await _cartService.GetSelectedPromotionAsync(customerId); // New addition
            return Ok(new { Cart = cart, SelectedPromotions = selectedPromotions });
        }

        [HttpGet("checkout-cart/{customerId}")]
        public async Task<IActionResult> GetCheckoutData(Guid customerId)
        {
            var checkoutData = await _reserveCartCacheService.GetCheckoutDataAsync(customerId);
            return Ok(checkoutData);
        }

        [HttpGet("checkout/{customerId}")]
        public async Task<IActionResult> CheckIfCustomerCheckout(Guid customerId)
        {
            var check = await _reserveCartCacheService.CheckIfCustomerCheckoutAsync(customerId);
            return Ok(check);
        }

        [HttpGet("cart-timer/{customerId}")]
        public async Task<IActionResult> GetCartTimer(Guid customerId)
        {
            var cartTimer = await _reserveCartCacheService.GetCartTimerAsync(customerId);
            return Ok(cartTimer);
        }


        [HttpPost("add/{customerId}")]
        public async Task<IActionResult> AddProductToCart(Guid customerId, [FromBody] CartItemReturnDto item)
        {
            await _cartService.AddProductToCartAsync(customerId, item);
            return Ok(new { Message = "Product added to cart successfully." });
        }

        [HttpPost("revalidate")]
        public async Task<IActionResult> RevalidateCart([FromBody] CartReturnDto clientCart)
        {
            var validatedCart = await _cartService.RevalidateCartWithCustomerIdAsync(clientCart);
            return Ok(validatedCart);
        }

        [HttpPost("select-promotion/{customerId}")]
        public async Task<IActionResult> SelectCustomerPromotion(Guid customerId, [FromBody] CustomerPromotionDto promotion)
        {
            var newCustomerPromotionList = await _cartService.SelectCustomerPromotion(customerId, promotion);
            return Ok(newCustomerPromotionList);
        }

        [HttpPost("remove-select-promotion/{customerId}")]
        public async Task<IActionResult> UnSelectCustomerPromotion(Guid customerId, [FromBody] List<CustomerPromotionDto> promotions)
        {
            var newCustomerPromotionList = await _cartService.UnSelectCustomerPromotion(customerId, promotions);
            return Ok(newCustomerPromotionList);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            await _reserveCartCacheService.CheckoutCartAsync(request.Cart, request.Promotions);
            await _reserveCartCacheService.ReduceStockOnCheckoutAsync(request.Cart);
            return Ok(new { Message = "Checkout successfully." });
        }


        [HttpPost("backtocart/{customerId}")]
        public async Task<IActionResult> BackToCart(Guid customerId)
        {
            await _reserveCartCacheService.RemoveReserveCartAsync(customerId);
            return Ok(new { Message = "Back to cart successfully." });
        }

        [HttpPost("readd-stock")]
        public async Task<IActionResult> ReaddStockForCartItem([FromBody] CartReturnDto cart)
        {
            await _reserveCartCacheService.AddStockOnNoPurchaseAsync(cart);
            return Ok(new { Message = "Stock readded successfully." });
        }

        /*        [HttpPatch("{customerId}/update/{productId}")]
                public async Task<IActionResult> UpdateProductQuantity(Guid customerId, Guid productId, [FromBody] int quantity)
                {
                    var cart = await _cartService.UpdateProductQuantityAsync(customerId, productId, quantity);
                    return Ok(cart);
                }*/

        [HttpDelete("remove/{customerId}/{productId}/{storeId}/{discountedPrice}/{quantity}")]
        public async Task<IActionResult> RemoveProductFromCart(Guid customerId, Guid productId, Guid storeId, double discountedPrice, int quantity)
        {
            await _cartService.RemoveProductFromCartAsync(customerId, productId, storeId, discountedPrice, quantity);
            return Ok(new { Message = "Product removed from cart successfully." });
        }

        [HttpDelete("clear/{customerId}")]
        public async Task<IActionResult> ClearCart(Guid customerId)
        {
            await _cartService.ClearCartAsync(customerId);
            return Ok(new { Message = "Product cleared from cart successfully." });
        }
    }
}
