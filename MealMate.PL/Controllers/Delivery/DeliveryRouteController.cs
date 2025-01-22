using MealMate.BLL.Dtos.Delivery;
using MealMate.BLL.IServices.Delivery;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers.Delivery
{
    [ApiController]
    [Route("delivery")]
    public class DeliveryRouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public DeliveryRouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpPost("optimize")]
        public async Task<IActionResult> OptimizeRoute([FromBody] RouteRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ShopAddress) || request.DeliveryAddresses == null || !request.DeliveryAddresses.Any())
                return BadRequest("Invalid input data.");

            try
            {
                var result = await _routeService.GetOptimalRouteAsync(request.ShopAddress, request.DeliveryAddresses);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
