using MealMate.BLL.Dtos.Promotion;

namespace MealMate.BLL.IServices
{
    public interface IBillPromotionAppService
    {
        Task<List<BillPromotionCreationDto>> GetAllBillPromotionsAsync();
        Task<BillPromotionCreationDto> GetBillPromotionByIdAsync(Guid id);
        Task<List<BillPromotionCreationDto>> GetPromotionsByBillId(Guid transactionId);
        Task<BillPromotionCreationDto> CreateBillPromotionAsync(BillPromotionCreationDto promotionData);
        Task<BillPromotionCreationDto> GetBestBillPromotionByPriceAsync(decimal totalprice);
        Task<int> ApplyBillPromotionToBillAsync(Guid promotionId, Guid billId);
        /*        Task DeletePromotionAsync(Guid id);*/
        Task DeleteExpiredPromotionsAsync();
    }
}
