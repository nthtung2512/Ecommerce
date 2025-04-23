using MealMate.BLL.Dtos.Bills;
using MealMate.DAL.Repositories;
using MealMate.DAL.Utils.Enum;

namespace MealMate.BLL.IServices
{
    public interface ITransactionService
    {
        // Bill
        Task<List<BillDto>> GetAllBillAsync();
        Task<List<BillDto>> GetBillListAsync(Guid customerId);
        Task<FullBillDto> GetBillByIdAsync(Guid transactionId);
        Task<List<FullBillDto>> GetBillListByStoreIdAsync(Guid storeId);
        Task<BillStatusStatisticsDto> GetBillStatusByStoreIdPrevAsync(Guid storeId);
        Task<List<FullBillDto>> GetBillListByStoreIdAndStatusAsync(Guid storeId, DeliveryStatus status);
        Task<Guid> GetLastBillIdAsync(Guid customerId);
        Task<FullBillDto> CreateBillAsync(BillCreationDto billData);
        Task<BillDto> AssignShipperToBillAsync(Guid transactionId, Guid shipperId);
        Task<DeliveryStatus> UpdateDeliveryStatusAsync(Guid transactionId, DeliveryStatus status);
        Task<CancelOrderDto> CancelOrderAsync(Guid billId, DeliveryStatus deliveryStatus);
        Task<List<BillDto>> GetBillListByStatusAsync(DeliveryStatus status);
        Task<List<ChatbotHistoryBillDto>> GetCustomerPurchaseHistory(Guid customerId);

        // Test bill
        Task CreateBulkBillPrevDay(Guid storeId);
    }
}
