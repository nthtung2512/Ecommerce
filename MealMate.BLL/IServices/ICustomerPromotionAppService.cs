using MealMate.BLL.Dtos.Promotion;

namespace MealMate.BLL.IServices
{
    public interface ICustomerPromotionAppService
    {
        Task<CustomerPromotionDto> GetCustomerPromotionByIdAsync(Guid promotionid);
        Task<CustomerPromotionDto> CreateCustomerPromotionAsync(CustomerPromotionCreationDto customerPromotionCreationDto);
        Task AssignPromotionToCustomerAsync(Guid promotionId, Guid customerid);
        Task DeleteExpiredPromotionsAsync();
        Task DeleteCustomerPromotionAsync(Guid promotionid, Guid customerid);
        Task<List<CustomerPromotionDto>> GetListAsync();
        Task<List<CustomerPromotionDto>> GetListByCustomerIdAsync(Guid customerId);
        Task<List<CustomerPromotionDto>> GetDiscountByProductIdListAsync(List<Guid> productIdList);
    }
}
