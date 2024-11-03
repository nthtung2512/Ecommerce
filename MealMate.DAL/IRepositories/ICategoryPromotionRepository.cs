using MealMate.DAL.Entities.Promotion;

namespace MealMate.DAL.IRepositories
{
    public interface ICategoryPromotionRepository
    {
        Task<List<ProductCategoryPromotion>> GetAllCategoryPromotionsAsync();
        Task<ProductCategoryPromotion?> GetCategoryPromotionByIdAsync(Guid id);
        Task<List<ProductCategoryPromotion>> GetCategoryPromotionByCategoryAsync(string category);
        Task CreateAsync(ProductCategoryPromotion newPromotion);
        Task DeleteAsync(ProductCategoryPromotion productPromotion);
    }
}
