using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.Dtos.Employee;
using MealMate.BLL.Dtos.Shipper;
using MealMate.DAL.Entities.ApplicationUser;

namespace MealMate.BLL.IServices.auth
{
    public interface IApplicationUserAppService
    {
        Task<CustomerDto> RegisterCustomerAsync(CustomerCreationDto customerDto);
        Task<EmployeeCreationDto> RegisterStoreManagerAsync(EmployeeCreationDto storeManagerDto);
        Task<ShipperDto> RegisterShipperAsync(ShipperCreationDto shipperDto);
        Task<ApplicationUser> GetUserAsync(string email, string password);
    }
}
