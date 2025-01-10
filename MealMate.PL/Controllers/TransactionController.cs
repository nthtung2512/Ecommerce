using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.Dtos.Cart;
using MealMate.BLL.IServices;
using MealMate.BLL.IServices.Redis;
using MealMate.DAL.Utils.Enum;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IReserveCartCacheService _reserveCartCacheService;

    public TransactionController(ITransactionService transactionService, IReserveCartCacheService reserveCartCacheService)
    {
        _transactionService = transactionService;
        _reserveCartCacheService = reserveCartCacheService;
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

    [HttpGet("store/{storeid}/{status}")]
    public async Task<IActionResult> GetBillListByStoreIdAsync(Guid storeid, DeliveryStatus status)
    {
        var bills = await _transactionService.GetBillListByStoreIdAsync(storeid, status);
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

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CartReturnDto cart)
    {
        await _reserveCartCacheService.CheckoutCartAsync(cart);
        return Ok(new { Message = "Checkout successfully." });
    }

    [HttpPost("backtocart")]
    public async Task<IActionResult> BackToCart(Guid customerId)
    {
        await _reserveCartCacheService.RemoveReserveCartAsync(customerId);
        return Ok(new { Message = "Back to cart successfully." });
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