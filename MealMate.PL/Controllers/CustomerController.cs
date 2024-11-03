using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomerController(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerList()
        {
            var customers = await _customerAppService.GetListAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _customerAppService.GetByIdAsync(id);
            return Ok(customer);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCustomerInfoById(Guid id, [FromBody] CustomerUpdateDto data)
        {
            var result = await _customerAppService.UpdateAsync(id, data);
            return Ok(result);
        }

        [HttpPatch("totalmoney/{id}/{money}")]
        public async Task<IActionResult> AddTotalMoneySpentById(Guid id, decimal money)
        {
            var result = await _customerAppService.AddTotalMoneySpentByIdAsync(id, money);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreationDto data)
        {
            var customer = await _customerAppService.CreateAsync(data);
            return Ok(customer);
        }

        [HttpGet("login/{email}/{password}")]
        public async Task<IActionResult> GetCustomerForLogin(string email, string password)
        {
            var customer = await _customerAppService.GetCustomerForLoginAsync(email, password);
            return Ok(customer);
        }

        [HttpGet("customer-rank/{customerID}")]
        public async Task<IActionResult> GetCustomerRank(Guid customerID)
        {
            var totalMoneySpent = (await _customerAppService.GetByIdAsync(customerID)).TotalMoneySpent;

            string rank = totalMoneySpent switch
            {
                >= 100000000 => "platinum",
                >= 50000000 => "gold",
                >= 25000000 => "silver",
                >= 10000000 => "iron",
                _ => "none"
            };

            return Ok(new { rank });
        }

        [HttpGet("lastid")]
        public async Task<IActionResult> GetLastCustomerID()
        {
            try
            {
                var lastCustomerId = await _customerAppService.GetLastCustomerIdAsync();
                return Ok(lastCustomerId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error retrieving last customer ID", details = ex.Message });
            }
        }
    }
}
