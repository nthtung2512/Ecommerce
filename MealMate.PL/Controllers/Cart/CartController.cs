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

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCart(Guid customerId)
        {
            var cart = await _cartService.GetCartAsync(customerId);
            return Ok(cart);
        }

        [HttpPost("{customerId}/add")]
        public async Task<IActionResult> AddProductToCart(Guid customerId, [FromBody] CartItem item)
        {
            var cart = await _cartService.AddProductToCartAsync(customerId, item);
            return Ok(cart);
        }

        [HttpPost("{customerId}/revalidate")]
        public async Task<IActionResult> RevalidateCart(Guid customerId)
        {
            var cart = await _cartService.RevalidateCartWithCustomerIdAsync(customerId);
            return Ok(cart);
        }

        [HttpPatch("{customerId}/update/{productId}")]
        public async Task<IActionResult> UpdateProductQuantity(Guid customerId, Guid productId, [FromBody] int quantity)
        {
            var cart = await _cartService.UpdateProductQuantityAsync(customerId, productId, quantity);
            return Ok(cart);
        }

        [HttpDelete("{customerId}/remove/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(Guid customerId, Guid productId)
        {
            var cart = await _cartService.RemoveProductFromCartAsync(customerId, productId);
            return Ok(cart);
        }

        [HttpDelete("{customerId}/clear")]
        public async Task<IActionResult> ClearCart(Guid customerId)
        {
            var cart = await _cartService.ClearCartAsync(customerId);
            return Ok(cart);
        }
    }
}
