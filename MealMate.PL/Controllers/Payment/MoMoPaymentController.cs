using MealMate.BLL.IServices.Payment;
using MealMate.DAL.Entities.Payment;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers.Payment
{
    [ApiController]
    [Route("payment/momo")]
    public class MoMoPaymentController : ControllerBase
    {
        private readonly IMomoService _momoService;
        public MoMoPaymentController(IMomoService momoService)
        {
            _momoService = momoService;
        }

        [HttpPost]
        [Route("createpayment")]
        public async Task<IActionResult> CreatePaymentAsync([FromBody] OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentAsync(model);
            return Ok(response);
        }

        [HttpGet]
        [Route("execute")]
        public IActionResult PaymentExecuteAsync([FromQuery] IQueryCollection collection)
        {
            var response = _momoService.PaymentExecuteAsync(collection);
            return Ok(response);
        }
    }
}
