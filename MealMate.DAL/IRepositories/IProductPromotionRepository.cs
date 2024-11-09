using MealMate.DAL.Entities.Promotion;

namespace MealMate.DAL.IRepositories
{
    public interface IProductPromotionRepository
    {
        Task<List<ProductPromotion>> GetAllProductPromotionsAsync();
        /* Task<ProductPromotion?> GetProductPromotionByIdAsync(Guid id);*/
        Task<List<ProductPromotion>> GetPromotionByProductIdAsync(Guid productId);
        /* Task<List<PromoteProduct>> GetPromotionInfoByProductIdAsync(Guid productId);*/
        Task CreateAsync(ProductPromotion newPromotion);
        Task DeleteAsync(ProductPromotion productPromotion);
        Task<List<ProductPromotion>> GetExpiredPromotionsAsync();
    }
}
