using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("promotions/bill")]
    public class BillPromotionController : ControllerBase
    {
        private readonly IBillPromotionAppService _billPromotionAppService;

        public BillPromotionController(IBillPromotionAppService billPromotionAppService)
        {
            _billPromotionAppService = billPromotionAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBillPromotions()
        {
            var promotions = await _billPromotionAppService.GetAllBillPromotionsAsync();
            return Ok(promotions);
        }

        [HttpGet("promotion/{promotionId}")]
        public async Task<IActionResult> GetPromotionByPromotionId(Guid promotionId)
        {
            var promotion = await _billPromotionAppService.GetBillPromotionByIdAsync(promotionId);
            return Ok(promotion);
        }

        [HttpGet("{billId}")]
        public async Task<IActionResult> GetPromotionByBillId(Guid billId)
        {
            var promotion = await _billPromotionAppService.GetPromotionsByBillId(billId);
            return Ok(promotion);
        }

        [HttpGet("price/{totalprice}")]
        public async Task<IActionResult> GetBestBillPromotionByPrice(decimal totalprice)
        {
            var promotion = await _billPromotionAppService.GetBestBillPromotionByPriceAsync(totalprice);
            return Ok(promotion);
        }

        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion(Guid id)
        {
            await _billPromotionAppService.DeletePromotionAsync(id);

            return Ok(new { message = "Promotion deleted successfully." });
        }*/
        [HttpPost]
        public async Task<IActionResult> CreateBillPromotion([FromBody] BillPromotionCreationDto promotionData)
        {
            var createdPromotion = await _billPromotionAppService.CreateBillPromotionAsync(promotionData);
            return Ok(createdPromotion);
        }

        [HttpPost("{promotionid}/{billid}")]
        public async Task<IActionResult> ApplyBillPromotionToBill(Guid promotionid, Guid billid)
        {
            var newPromotionChance = await _billPromotionAppService.ApplyBillPromotionToBillAsync(promotionid, billid);
            return Ok(new
            {
                success = true,
                message = "Promotion applied successfully.",
                newPromotionChance
            });
        }
    }
}
