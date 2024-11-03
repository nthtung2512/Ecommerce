using MealMate.BLL.Dtos.Promotion;

namespace MealMate.BLL.IServices
{
    public interface ICategoryPromotionAppService
    {
        Task<List<CategoryPromotionCreationDto>> GetAllCategoryPromotionsAsync();
        Task<CategoryPromotionCreationDto> GetCategoryPromotionByIdAsync(Guid id);
        Task<List<CategoryPromotionCreationDto>> GetPromotionsByProductCategory(string category);
        Task<ProductPromotionCreationDto> CreateProductPromotionByCategoryAsync(CategoryPromotionCreationDto promotionData, string category);
        Task DeletePromotionAsync(Guid id);
    }
}
