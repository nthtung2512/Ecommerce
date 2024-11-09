using MealMate.BLL.Dtos.Promotion;

namespace MealMate.BLL.IServices
{
    public interface IProductPromotionAppService
    {
        Task<List<ProductPromotionDto>> GetAllProductPromotionsAsync();
        /* Task<ProductPromotionCreationDto> GetProductPromotionByIdAsync(Guid id);*/
        Task<List<ProductPromotionDto>> GetPromotionsByProductId(Guid productId);
        Task<ProductPromotionDto> CreateProductPromotionByProductIdAsync(ProductPromotionCreationDto promotionData);
        Task DeleteExpiredPromotionsAsync();
    }
}
