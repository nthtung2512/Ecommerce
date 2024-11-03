using MealMate.BLL.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers
{
    [ApiController]
    [Route("stores")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreAppService _storeAppService;

        public StoreController(IStoreAppService storeAppService)
        {
            _storeAppService = storeAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStores()
        {
            var stores = await _storeAppService.GetAllStoresAsync();
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(Guid id)
        {
            var store = await _storeAppService.GetStoreByIdAsync(id);
            return Ok(store);
        }
    }
}
