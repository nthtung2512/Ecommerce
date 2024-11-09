using MealMate.BLL.Dtos.Customer;

namespace MealMate.BLL.IServices
{
    public interface ICustomerAppService
    {
        Task<List<CustomerDto>> GetListAsync();
        Task<CustomerDto> GetByIdAsync(Guid customerId);
        Task<CustomerDto> UpdateAsync(
            Guid customerId,
            CustomerUpdateDto updateData
        );
        Task<CustomerDto> AddTotalMoneySpentByIdAsync(Guid id, decimal money);
        Task<Guid> GetLastCustomerIdAsync();
    }
}
