using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.IServices;
using MealMate.DAL.Utils.Enum;
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

    [HttpGet]
    public async Task<IActionResult> GetAllBills()
    {
        var bills = await _transactionService.GetAllBillAsync();
        return Ok(bills);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetBillListByCustomerIdAsync(Guid customerId)
    {
        var bills = await _transactionService.GetBillListAsync(customerId);
        return Ok(bills);
    }

    [HttpGet("{transactionId}")]
    public async Task<IActionResult> GetFullBillById(Guid transactionId)
    {
        var bill = await _transactionService.GetBillByIdAsync(transactionId);
        return Ok(bill);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetBillListByStatus(DeliveryStatus status)
    {
        var bills = await _transactionService.GetBillListByStatusAsync(status);
        return Ok(bills);
    }

    [HttpGet("store/{storeid}")]
    public async Task<IActionResult> GetBillListByStoreIdAsync(Guid storeid)
    {
        var bills = await _transactionService.GetBillListByStoreIdAsync(storeid);
        return Ok(bills);
    }

    [HttpGet("store/prev/{storeid}")]
    public async Task<IActionResult> GetBillStatusByStoreIdPrevAsync(Guid storeid)
    {
        var billStatuses = await _transactionService.GetBillStatusByStoreIdPrevAsync(storeid);
        return Ok(billStatuses);
    }

    [HttpGet("store/{storeid}/{status}")]
    public async Task<IActionResult> GetBillListByStoreIdAndStatusAsync(Guid storeid, DeliveryStatus status)
    {
        var bills = await _transactionService.GetBillListByStoreIdAndStatusAsync(storeid, status);
        return Ok(bills);
    }

    [HttpGet("last/{customerId}")]
    public async Task<IActionResult> GetLastBillId(Guid customerId)
    {
        var lastBillId = await _transactionService.GetLastBillIdAsync(customerId);
        return Ok(lastBillId);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBill([FromBody] BillCreationDto billData)
    {
        var newBillId = await _transactionService.CreateBillAsync(billData);
        return Ok(new { new_bill = newBillId });
    }

    [HttpPost("bulk/{storeId}")]
    public async Task<IActionResult> CreateBulkBill(Guid storeId)
    {
        await _transactionService.CreateBulkBillPrevDay(storeId);
        return Ok();
    }

    [HttpPatch("status/{transactionId}/{status}")]
    public async Task<IActionResult> UpdateDeliveryStatus(Guid transactionId, DeliveryStatus status)
    {
        var result = await _transactionService.UpdateDeliveryStatusAsync(transactionId, status);
        return Ok(result);
    }

    [HttpPatch("{transactionId}/{shipperId}")]
    public async Task<IActionResult> AssignShipperToBill(Guid transactionId, Guid shipperId)
    {
        var result = await _transactionService.AssignShipperToBillAsync(transactionId, shipperId);
        return Ok(result);
    }
}