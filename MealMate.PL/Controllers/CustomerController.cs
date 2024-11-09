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

        [HttpGet("customer-rank/{customerID}")]
        public async Task<IActionResult> GetCustomerRank(Guid customerID)
        {
            var totalMoneySpent = (await _customerAppService.GetByIdAsync(customerID)).TotalMoneySpent;

            string rank = totalMoneySpent switch
            {
                >= 10000 => "platinum",
                >= 5000 => "gold",
                >= 1500 => "silver",
                >= 500 => "iron",
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
