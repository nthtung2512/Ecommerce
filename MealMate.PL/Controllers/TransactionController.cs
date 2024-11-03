using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetBillListByCustomerIdAsync(Guid customerId)
    {
        try
        {
            var bills = await _transactionService.GetBillListAsync(customerId);
            return Ok(bills);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve bill", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateBill([FromBody] BillCreationDto billData)
    {
        try
        {
            var newBillId = await _transactionService.CreateBillAsync(billData);
            return Ok(new { new_bill = newBillId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to create bill", details = ex.Message });
        }
    }

    [HttpGet("{transactionId}")]
    public async Task<IActionResult> GetBillById(Guid transactionId)
    {
        try
        {
            var bill = await _transactionService.GetBillByIdAsync(transactionId);
            return Ok(bill);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve bill", details = ex.Message });
        }
    }

    [HttpGet("last/{customerId}")]
    public async Task<IActionResult> GetLastBillId(Guid customerId)
    {
        try
        {
            var lastBillId = await _transactionService.GetLastBillIdAsync(customerId);
            return Ok(lastBillId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve last bill ID", details = ex.Message });
        }
    }
}