using MealMate.DAL.Entities.Promotion;

namespace MealMate.DAL.IRepositories
{
    public interface IBillPromotionRepository
    {
        Task<List<BillPromotion>> GetPromotionByBillId(Guid billId);
        Task<List<BillPromotion>> GetAllBillPromotionsAsync();
        Task<BillPromotion?> GetBillPromotionByIdAsync(Guid id);
        Task CreateAsync(BillPromotion newPromotion);
        Task DeleteAsync(BillPromotion billPromotion);
    }
}