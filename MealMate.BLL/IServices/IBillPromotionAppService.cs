using MealMate.BLL.Dtos.Promotion;

namespace MealMate.BLL.IServices
{
    public interface IBillPromotionAppService
    {
        Task<List<BillPromotionCreationDto>> GetAllBillPromotionsAsync();
        Task<BillPromotionCreationDto> GetBillPromotionByIdAsync(Guid id);
        Task<List<BillPromotionCreationDto>> GetPromotionsByBillId(Guid transactionId);
        Task<BillPromotionCreationDto> CreateBillPromotionAsync(BillPromotionCreationDto promotionData);
        Task DeletePromotionAsync(Guid id);
    }
}
