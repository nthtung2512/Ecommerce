using MealMate.BLL.Dtos.Employee;

namespace MealMate.BLL.IServices
{
    public interface IEmployeeAppService
    {
        Task<List<EmployeeDto>> GetListEmployeeAsync();
        Task<EmployeeDto> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeDto> GetEmployeeByStoreIdAsync(Guid storeid);
        Task<EmployeeDto> UpdateEmployeeAsync(Guid id, EmployeeUpdateDto updateData);
        Task DeleteEmployeeAsync(Guid id);
    }
}
