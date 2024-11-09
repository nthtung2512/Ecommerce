using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("/promotions/customer")]
    public class CustomerPromotionController : ControllerBase
    {
        private readonly ICustomerPromotionAppService _customerPromotionAppService;

        public CustomerPromotionController(ICustomerPromotionAppService customerPromotionAppService)
        {
            _customerPromotionAppService = customerPromotionAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomerPromotions()
        {
            var promotions = await _customerPromotionAppService.GetListAsync();
            return Ok(promotions);
        }

        [HttpGet("{customerid}")]
        public async Task<IActionResult> GetCustomerPromotionsByCustomerId(Guid customerid)
        {
            var promotion = await _customerPromotionAppService.GetListByCustomerIdAsync(customerid);
            return Ok(promotion);
        }

        [HttpGet("promotion/{promotionId}")]
        public async Task<IActionResult> GetCustomerPromotionById(Guid promotionId)
        {
            var promotion = await _customerPromotionAppService.GetCustomerPromotionByIdAsync(promotionId);
            return Ok(promotion);
        }

        [HttpPost("product")]
        public async Task<IActionResult> GetDiscountByProductIdList([FromBody] List<Guid> productIdList)
        {
            var promotions = await _customerPromotionAppService.GetDiscountByProductIdListAsync(productIdList);
            return Ok(promotions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerPromotion(CustomerPromotionCreationDto promotionData)
        {
            var promotion = await _customerPromotionAppService.CreateCustomerPromotionAsync(promotionData);
            return Ok(promotion);
        }

        [HttpPost("{promotionId}/{customerid}")]
        public async Task<IActionResult> AssignPromotionToCustomer(Guid promotionId, Guid customerid)
        {
            await _customerPromotionAppService.AssignPromotionToCustomerAsync(promotionId, customerid);
            return Ok(new
            {
                success = true,
                message = "Promotion applied successfully.",
            });
        }

        [HttpDelete("{promotionid}/{customerid}")]
        public async Task<IActionResult> DeleteCustomerPromotionAfterUsage(Guid promotionid, Guid customerid)
        {
            await _customerPromotionAppService.DeleteCustomerPromotionAsync(promotionid, customerid);
            return Ok(new
            {
                success = true,
                message = "Promotion deleted successfully.",
            });
        }
    }
}
