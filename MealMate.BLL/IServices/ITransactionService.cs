using MealMate.BLL.Dtos.Bills;

namespace MealMate.BLL.IServices
{
    public interface ITransactionService
    {
        // Bill
        Task<List<BillCreationDto>> GetBillListAsync(Guid customerId);
        Task<BillCreationDto> GetBillByIdAsync(Guid transactionId);
        Task<Guid> GetLastBillIdAsync(Guid customerId);
        Task<BillCreationDto> CreateBillAsync(BillCreationDto billData);
        Task<BillCreationDto> AssignShipperAsync(BillCreationDto assignedBill);
    }
}
