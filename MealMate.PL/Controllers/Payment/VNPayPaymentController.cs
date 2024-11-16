using MealMate.BLL.IServices.Payment;
using MealMate.DAL.Entities.Payment.VNPay;
using Microsoft.AspNetCore.Mvc;

namespace MealMate.PL.Controllers.Payment
{
    [ApiController]
    [Route("payment/vnpay")]
    public class VnPayPaymentController : Controller
    {

        private readonly IVnPayService _vnPayService;
        public VnPayPaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost]
        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(url);
        }
        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            // From URL
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Ok(response);
        }


    }


}
