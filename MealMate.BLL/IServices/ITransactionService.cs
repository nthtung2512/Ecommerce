using MealMate.BLL.Dtos.Bills;
using MealMate.DAL.Utils.Enum;

namespace MealMate.BLL.IServices
{
    public interface ITransactionService
    {
        // Bill
        Task<List<BillDto>> GetAllBillAsync();
        Task<List<BillDto>> GetBillListAsync(Guid customerId);
        Task<FullBillDto> GetBillByIdAsync(Guid transactionId);
        Task<Guid> GetLastBillIdAsync(Guid customerId);
        Task<FullBillDto> CreateBillAsync(BillCreationDto billData);
        Task<BillDto> AssignShipperToBillAsync(Guid transactionId, Guid shipperId);
        Task<DeliveryStatus> UpdateDeliveryStatusAsync(Guid transactionId, DeliveryStatus status);
    }
}
