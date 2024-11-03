using MealMate.BLL.Dtos.Promotion;

namespace MealMate.BLL.IServices
{
    public interface IProductPromotionAppService
    {
        Task<List<ProductPromotionCreationDto>> GetAllProductPromotionsAsync();
        Task<ProductPromotionCreationDto> GetProductPromotionByIdAsync(Guid id);
        Task<List<ProductPromotionCreationDto>> GetPromotionsByProductId(Guid productId);
        Task<ProductPromotionCreationDto> CreateProductPromotionByProductIdAsync(ProductPromotionCreationDto promotionData, Guid productId);
        Task DeletePromotionAsync(Guid id);
    }
}
