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
