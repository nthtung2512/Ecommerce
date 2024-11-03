using MealMate.BLL.Dtos.Employee;

namespace MealMate.BLL.IServices
{
    public interface IEmployeeAppService
    {
        /*        Task<EmployeeCreationDto> GetEmployeeByIdAsync(Guid id);*/
        // TODO: MOve to auth
        Task<EmployeeCreationDto> GetStoreManagerForLoginAsync(string email, string password);
        /*Task<StoreManagerCreationDto> CreateStoreManagerAsync(StoreManagerCreationDto storeManagerData);*/
        Task<EmployeeCreationDto> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeCreationDto> UpdateEmployeeAsync(Guid id, EmployeeUpdateDto updateData);
        Task DeleteEmployeeAsync(Guid id);
    }
}
