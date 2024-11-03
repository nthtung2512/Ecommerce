using MealMate.BLL.Dtos.Shipper;
using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("shippers")]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperAppService _shipperAppService;
        private readonly ITransactionService _transactionService;
        public ShipperController(IShipperAppService shipperAppService, ITransactionService transactionService)
        {
            _shipperAppService = shipperAppService;
            _transactionService = transactionService;
        }

        [HttpGet("{shipperId}")]
        public async Task<IActionResult> GetShipperById(Guid shipperId)
        {
            var shipper = await _shipperAppService.GetByIdAsync(shipperId);
            return Ok(shipper);
        }

        [HttpGet]
        public async Task<IActionResult> GetFreeShipperByArea([FromBody] string area)
        {
            var shipper = await _shipperAppService.GetFreeShipperByAreaAsync(area);
            return Ok(shipper);
        }

        [HttpPatch("assign/{transactionId}")]
        public async Task<IActionResult> AssignShipper(Guid transactionId)
        {
            var assignedBill = await _transactionService.GetBillByIdAsync(transactionId);
            var result = await _transactionService.AssignShipperAsync(assignedBill);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipper([FromBody] ShipperCreationDto shipperData)
        {
            var result = await _shipperAppService.CreateShipperAsync(shipperData);
            return Ok(result);
        }

        [HttpPatch("{shipperId}")]
        public async Task<IActionResult> UpdateShipper(Guid shipperId, [FromBody] ShipperUpdateDto shipperData)
        {
            var result = await _shipperAppService.UpdateShipperAsync(shipperId, shipperData);
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
