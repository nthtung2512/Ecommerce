using MealMate.BLL.Dtos.Customer;

namespace MealMate.BLL.IServices
{
    public interface ICustomerAppService
    {
        Task<List<CustomerDto>> GetListAsync();
        Task<CustomerDto> GetByIdAsync(Guid customerId);
        Task<CustomerDto> CreateAsync(CustomerCreationDto createData);
        Task<CustomerDto> UpdateAsync(
            Guid customerId,
            CustomerUpdateDto updateData
        );
        Task<CustomerDto> AddTotalMoneySpentByIdAsync(Guid id, decimal money);
        // TODO: Move to auth
        Task<CustomerDto> GetCustomerForLoginAsync(string email, string password);
        Task<Guid> GetLastCustomerIdAsync();
        Task DeleteAsync(Guid customerId);
    }
}
