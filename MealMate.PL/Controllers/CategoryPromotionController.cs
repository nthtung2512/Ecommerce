using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("promotions/category")]
    public class CategoryPromotionController : ControllerBase
    {
        private readonly ICategoryPromotionAppService _categoryPromotionAppService;

        public CategoryPromotionController(ICategoryPromotionAppService categoryPromotionAppService)
        {
            _categoryPromotionAppService = categoryPromotionAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPromotions()
        {
            var promotions = await _categoryPromotionAppService.GetAllCategoryPromotionsAsync();
            return Ok(promotions);
        }

        [HttpGet("promotion/{promotionId}")]
        public async Task<IActionResult> GetPromotionByPromotionId(Guid promotionId)
        {
            var promotion = await _categoryPromotionAppService.GetCategoryPromotionByIdAsync(promotionId);
            return Ok(promotion);
        }

        [HttpGet("{category}")]
        public async Task<IActionResult> GetPromotionByProductCategory(string category)
        {
            var promotion = await _categoryPromotionAppService.GetPromotionsByProductCategory(category);
            return Ok(promotion);
        }

        [HttpPost("{category}")]
        public async Task<IActionResult> CreateProductPromotionByCategory([FromBody] CategoryPromotionCreationDto promotionData, string category)
        {
            var createdPromotion = await _categoryPromotionAppService.CreateProductPromotionByCategoryAsync(promotionData, category);
            return Ok(createdPromotion);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion(Guid id)
        {
            await _categoryPromotionAppService.DeletePromotionAsync(id);

            return Ok(new { message = "Promotion deleted successfully." });
        }
    }
}
