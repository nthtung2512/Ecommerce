using MealMate.BLL.Dtos.Shipper;
using MealMate.BLL.IServices;
using MealMate.DAL.IRepositories.UnitOfWork;
using MealMate.DAL.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("shippers")]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperAppService _shipperAppService;
        private readonly ITransactionService _transactionService;

        private readonly IUnitOfWork _unitOfWork;

        public ShipperController(IShipperAppService shipperAppService, ITransactionService transactionService, IUnitOfWork unitOfWork)
        {
            _shipperAppService = shipperAppService;
            _transactionService = transactionService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllShippers()
        {
            var shipper = await _shipperAppService.GetListAsync();
            return Ok(shipper);
        }

        [HttpGet("{shipperId}")]
        public async Task<IActionResult> GetShipperById(Guid shipperId)
        {
            var shipper = await _shipperAppService.GetByIdAsync(shipperId);
            return Ok(shipper);
        }

        [HttpGet("phone/{phoneno}")]
        public async Task<IActionResult> GetShipperByPhoneNumber(string phoneno)
        {
            var shipper = await _shipperAppService.GetShipperByPhoneNumberAsync(phoneno);
            return Ok(shipper);
        }

        [HttpPost("cancel-order/{billId}/{shipperId}")]
        public async Task<IActionResult> CancelOrder(Guid billId, Guid shipperId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _transactionService.CancelOrderAsync(billId, DeliveryStatus.Prepared);
                await _shipperAppService.UpdateShipperCapacityAsync(shipperId, -result.TotalWeight);

                await _unitOfWork.CommitTransactionAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("get-bombed/{billId}/{shipperId}")]
        public async Task<IActionResult> GetBombed(Guid billId, Guid shipperId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _transactionService.CancelOrderAsync(billId, DeliveryStatus.Ghost);
                var updatedShipper = await _shipperAppService.UpdateShipperCapacityAsync(shipperId, -result.TotalWeight);
                await _unitOfWork.CommitTransactionAsync();
                return Ok(updatedShipper);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("customer-cancel-order/{billId}/{shipperId}")]
        [SwaggerOperation(
           Summary = "Customer cancel order",
           Description = "Return void"
        )]
        public async Task<IActionResult> CancelOrderByCustomer(Guid billId, Guid shipperId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _transactionService.CancelOrderAsync(billId, DeliveryStatus.Cancelled);
                var updatedShipper = await _shipperAppService.UpdateShipperCapacityAsync(shipperId, -result.TotalWeight);
                await _unitOfWork.CommitTransactionAsync();
                return Ok(updatedShipper);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{shipperId}")]
        public async Task<IActionResult> UpdateShipper(Guid shipperId, [FromBody] ShipperUpdateDto shipperData)
        {
            var result = await _shipperAppService.UpdateShipperAsync(shipperId, shipperData);
            return Ok(result);
        }

        [HttpPatch("{shipperId}/{capacity}")]
        public async Task<IActionResult> UpdateShipperCapacity(Guid shipperId, int capacity)
        {
            var result = await _shipperAppService.UpdateShipperCapacityAsync(shipperId, capacity);
            return Ok(result);
        }

        [HttpDelete("{shipperId}")]
        public async Task<IActionResult> DeleteShipper(Guid shipperId)
        {
            await _shipperAppService.DeleteShipperAsync(shipperId);
            return Ok();
        }
    }
}
