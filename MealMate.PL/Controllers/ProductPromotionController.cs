﻿using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("promotions/product")]
    public class ProductPromotionController : ControllerBase
    {
        private readonly IProductPromotionAppService _productPromotionAppService;

        public ProductPromotionController(IProductPromotionAppService productPromotionAppService)
        {
            _productPromotionAppService = productPromotionAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPromotions()
        {
            var promotions = await _productPromotionAppService.GetAllProductPromotionsAsync();
            return Ok(promotions);
        }

        /*        [HttpGet("promotion/{promotionId}")]
                public async Task<IActionResult> GetPromotionByPromotionId(Guid promotionId)
                {
                    var promotion = await _productPromotionAppService.GetProductPromotionByIdAsync(promotionId);
                    return Ok(promotion);
                }*/

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetPromotionByProductId(Guid productId)
        {
            var promotion = await _productPromotionAppService.GetPromotionsByProductId(productId);
            return Ok(promotion);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductPromotionByProductId([FromBody] ProductPromotionCreationDto promotionData)
        {
            var createdPromotion = await _productPromotionAppService.CreateProductPromotionByProductIdAsync(promotionData);
            return Ok(createdPromotion);
        }
    }
}
