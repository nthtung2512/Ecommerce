using MealMate.BLL.Dtos.Customer;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "Get all customers",
            Description = "Return: Guid Id; string Address; string FName; string LName; string PhoneNumber; string Email; decimal TotalMoneySpent; int FortuneChance"
        )]
        public async Task<IActionResult> GetCustomerList()
        {
            var customers = await _customerAppService.GetListAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get customer by customer id",
            Description = "Return: Guid Id; string Address; string FName; string LName; string PhoneNumber; string Email; decimal TotalMoneySpent; int FortuneChance"
        )]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _customerAppService.GetByIdAsync(id);
            return Ok(customer);
        }

        [HttpPatch("{id}")]
        [SwaggerOperation(
            Summary = "Update customer information by customer id",
            Description = "Return: Guid Id; string Address; string FName; string LName; string PhoneNumber; string Email; decimal TotalMoneySpent; int FortuneChance"
        )]
        public async Task<IActionResult> UpdateCustomerInfoById(Guid id, [FromBody] CustomerUpdateDto data)
        {
            var result = await _customerAppService.UpdateAsync(id, data);
            return Ok(result);
        }

        [HttpPatch("totalmoney/{id}/{money}")]
        [SwaggerOperation(
            Summary = "Add total money spent by customer id",
            Description = "Return: Guid Id; string Address; string FName; string LName; string PhoneNumber; string Email; decimal TotalMoneySpent; int FortuneChance"
        )]
        public async Task<IActionResult> AddTotalMoneySpentById(Guid id, decimal money)
        {
            var result = await _customerAppService.AddTotalMoneySpentByIdAsync(id, money);
            return Ok(result);
        }

        [HttpGet("customer-rank/{customerID}")]
        [SwaggerOperation(
            Summary = "Get customer rank",
            Description = "Return: string rank"
        )]
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
        [SwaggerOperation(
            Summary = "Get last customer id",
            Description = "Return: Guid CustomerId"
        )]
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
