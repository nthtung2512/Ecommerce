using MealMate.DAL.Entities.Promotion;

namespace MealMate.DAL.IRepositories
{
    public interface IBillPromotionRepository
    {
        Task<List<BillPromotion>> GetPromotionByBillId(Guid billId);
        Task<List<BillPromotion>> GetAllBillPromotionsAsync();
        Task<BillPromotion?> GetBillPromotionByIdAsync(Guid id);
        Task<BillPromotion?> GetBestBillPromotionByPriceAsync(decimal totalprice);
        Task UpdateAsync(BillPromotion billPromotion);
        Task CreateAsync(BillPromotion newPromotion);
        /*     Task DeleteAsync(BillPromotion billPromotion);*/
        Task<List<BillPromotion>> GetExpiredBillPromotions();
        Task DeleteExpiredPromotionsAsync(BillPromotion billPromotion);
    }
}