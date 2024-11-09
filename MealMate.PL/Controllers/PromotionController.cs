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
