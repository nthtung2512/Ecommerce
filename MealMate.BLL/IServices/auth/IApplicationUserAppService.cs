using MealMate.BLL.Dtos.ApplicationUser;
using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.Dtos.Employee;
using MealMate.BLL.Dtos.Shipper;

namespace MealMate.BLL.IServices.auth
{
    public interface IApplicationUserAppService
    {
        Task<CustomerDto> RegisterCustomerAsync(CustomerCreationDto customerDto);
        Task<EmployeeDto> RegisterStoreManagerAsync(EmployeeCreationDto storeManagerDto);
        Task<ShipperDto> RegisterShipperAsync(ShipperCreationDto shipperDto);
        Task<UserWithRoleDto> GetUserAsync(string email, string password);
    }
}
