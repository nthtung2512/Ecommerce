using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("promotions")]
    public class PromotionController : ControllerBase
    {
        private readonly IProductPromotionAppService _productPromotionAppService;
        private readonly IBillPromotionAppService _billPromotionAppService;
        private readonly ICategoryPromotionAppService _categoryPromotionAppService;
        private readonly ICustomerPromotionAppService _customerPromotionAppService;

        public PromotionController(IProductPromotionAppService productPromotionAppService, IBillPromotionAppService billPromotionAppService, ICategoryPromotionAppService categoryPromotionAppService, ICustomerPromotionAppService customerPromotionAppService)
        {
            _productPromotionAppService = productPromotionAppService;
            _billPromotionAppService = billPromotionAppService;
            _categoryPromotionAppService = categoryPromotionAppService;
            _customerPromotionAppService = customerPromotionAppService;
        }

        [HttpDelete("{type}/{id}")]
        public async Task<IActionResult> DeletePromotion(string type, Guid id)
        {
            if (string.IsNullOrEmpty(type))
            {
                return BadRequest(new { message = "Promotion type is required." });
            }
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Promotion ID is required." });
            }
            if (type.Equals("Product Promotion", StringComparison.OrdinalIgnoreCase))
            {
                await _productPromotionAppService.DeleteProductPromotionAsync(id);
            }
            else if (type.Equals("Bill Promotion", StringComparison.OrdinalIgnoreCase))
            {
                await _billPromotionAppService.DeleteBillPromotionAsync(id);
            }
            else if (type.Equals("Customer Promotion", StringComparison.OrdinalIgnoreCase))
            {
                await _customerPromotionAppService.DeleteCustomerPromotionAdminAsync(id);
            }
            else
            {
                return BadRequest(new { message = "Invalid promotion type." });
            }

            return Ok(new { message = "Promotion deleted successfully." });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExpiredPromotions()
        {
            await _productPromotionAppService.DeleteExpiredPromotionsAsync();
            await _billPromotionAppService.DeleteExpiredPromotionsAsync();
            /*            await _categoryPromotionAppService.DeleteExpiredPromotionsAsync();*/
            await _customerPromotionAppService.DeleteExpiredPromotionsAsync();

            return Ok(new { message = "Expired promotions deleted successfully." });
        }
    }
}
