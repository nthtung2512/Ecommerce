using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.Dtos.Employee;
using MealMate.BLL.Dtos.Shipper;
using MealMate.BLL.IServices.auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

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
        [SwaggerOperation(
            Summary = "User login",
            Description = "Login by email and password then return Cookie. Cookie has 3 claim: Id, Username, Role"
        )]
        public async Task<IActionResult> Login(string email, string password)
        {
            var userWithRole = await _applicationUserAppService.GetUserAsync(email, password);
            // Sign in user to create authentication cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userWithRole.User.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{userWithRole.User.FName} {userWithRole.User.LName}"),
                new Claim(ClaimTypes.Role, userWithRole.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Keeps cookie even after browser close
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(6) // Sets the 6-hour expiration
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
            return Ok(userWithRole);
        }

        [HttpPost("/register/customer")]
        [SwaggerOperation(
            Summary = "Register customer",
            Description = "Return: Guid Id; string Address; string FName; string LName; string PhoneNumber; string Email; decimal TotalMoneySpent; int FortuneChance"
        )]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerCreationDto customerDto)
        {
            var customer = await _applicationUserAppService.RegisterCustomerAsync(customerDto);
            return Ok(customer);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/register/storemanager")]
        [SwaggerOperation(
            Summary = "Register storemanager",
            Description = "Return: Guid Id; string Address; string FName; string LName; string PhoneNumber; string Email; double Salary; Guid StoreID"
        )]
        public async Task<IActionResult> RegisterStoreManager([FromBody] EmployeeCreationDto storeManagerDto)
        {
            var storeManager = await _applicationUserAppService.RegisterStoreManagerAsync(storeManagerDto);
            return Ok(storeManager);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/register/shipper")]
        [SwaggerOperation(
            Summary = "Register shipper",
            Description = "Return: Guid Id; string Address; string FName; string LName; string PhoneNumber; string Email; int VehicleCapacity"
        )]
        public async Task<IActionResult> RegisterShipper([FromBody] ShipperCreationDto shipperDto)
        {
            var shipper = await _applicationUserAppService.RegisterShipperAsync(shipperDto);
            return Ok(shipper);
        }
    }

}
