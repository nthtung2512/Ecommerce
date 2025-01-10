using MealMate.BLL.Dtos.Cart;
using MealMate.BLL.IServices.Redis;
using MealMate.DAL.Entities.Transactions;
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
            return Ok(cart);
        }

        [HttpGet("checkout/{customerId}")]
        public async Task<IActionResult> CheckIfCustomerCheckout(Guid customerId)
        {
            var check = await _reserveCartCacheService.CheckIfCustomerCheckoutAsync(customerId);
            return Ok(check);
        }

        [HttpPost("add/{customerId}")]
        public async Task<IActionResult> AddProductToCart(Guid customerId, [FromBody] CartItem item)
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
