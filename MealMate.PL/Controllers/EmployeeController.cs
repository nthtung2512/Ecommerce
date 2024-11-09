using MealMate.BLL.Dtos.Employee;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeAppService _employeeAppService;

        public EmployeeController(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListEmployees()
        {
            var employee = await _employeeAppService.GetListEmployeeAsync();
            return Ok(employee);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var employee = await _employeeAppService.GetEmployeeByIdAsync(id);
            return Ok(employee);
        }

        [HttpGet("store/{storeid}")]
        public async Task<IActionResult> GetEmployeeByStoreId(Guid storeid)
        {
            var employee = await _employeeAppService.GetEmployeeByStoreIdAsync(storeid);
            return Ok(employee);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeUpdateDto updateData)
        {
            var updatedEmployee = await _employeeAppService.UpdateEmployeeAsync(id, updateData);
            return Ok(updatedEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                await _employeeAppService.DeleteEmployeeAsync(id);
                return Ok(new { data = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error deleting employee", details = ex.Message });
            }
        }
    }
}
