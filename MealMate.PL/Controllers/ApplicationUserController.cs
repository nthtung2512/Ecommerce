using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.Dtos.Employee;
using MealMate.BLL.Dtos.Shipper;
using MealMate.BLL.IServices.auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// change this to auth later
namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("users")]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserAppService _applicationUserAppService;

        public ApplicationUserController(IApplicationUserAppService applicationUserAppService)
        {
            _applicationUserAppService = applicationUserAppService;
        }

        [HttpGet("/login/{email}/{password}")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var users = await _applicationUserAppService.GetUserAsync(email, password);
            return Ok(users);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("/register/customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerCreationDto customerDto)
        {
            var customer = await _applicationUserAppService.RegisterCustomerAsync(customerDto);
            return Ok(customer);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/register/storemanager")]
        public async Task<IActionResult> RegisterStoreManager([FromBody] EmployeeCreationDto storeManagerDto)
        {
            var storeManager = await _applicationUserAppService.RegisterStoreManagerAsync(storeManagerDto);
            return Ok(storeManager);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/register/shipper")]
        public async Task<IActionResult> RegisterShipper([FromBody] ShipperCreationDto shipperDto)
        {
            var shipper = await _applicationUserAppService.RegisterShipperAsync(shipperDto);
            return Ok(shipper);
        }
    }

}
