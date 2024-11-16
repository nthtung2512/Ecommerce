using MealMate.BLL.IServices.Payment;
using MealMate.BLL.Libraries;
using MealMate.DAL.Entities.Payment.VNPay;
using MealMate.DAL.Utils.GuidUtil;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MealMate.BLL.Services.Payment
{
    internal class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly GuidGenerator _guidGenerator;
        public VnPayService(IConfiguration configuration, GuidGenerator guidGenerator)
        {
            _configuration = configuration;
            _guidGenerator = guidGenerator;
        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            /*            var tick = DateTime.Now.Ticks.ToString();*/
            var orderId = _guidGenerator.Create().ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["Vnpay:PaymentBackReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Customer: {model.Name} \n Order Description: {model.OrderDescription} \n Total Money {model.Amount}");
            /*            pay.AddRequestData("vnp_OrderInfo", model.OrderDescription);*/
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", orderId);
            /*            pay.AddRequestData("vnp_TxnRef", tick);*/

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;
        }


    }
}
