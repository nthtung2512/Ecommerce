using MealMate.BLL.IServices.Payment;
using MealMate.BLL.Libraries;
using MealMate.DAL.Entities.Payment.VNPay;
using MealMate.DAL.Utils.GuidUtil;
using Microsoft.AspNetCore.Http;

namespace MealMate.BLL.Services.Payment
{
    internal class VnPayService : IVnPayService
    {
        private readonly GuidGenerator _guidGenerator;

        public VnPayService(GuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
        }

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Directly use timezone ID
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

            var orderId = _guidGenerator.Create().ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = "https://mealmate-seven.vercel.app/Checkout/PaymentCallBack"; // Direct callback URL

            pay.AddRequestData("vnp_Version", "2.1.0"); // Direct version value
            pay.AddRequestData("vnp_Command", "pay"); // Direct command value
            pay.AddRequestData("vnp_TmnCode", "DP02D80T"); // Direct TmnCode value
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND"); // Direct currency code
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", "en"); // Direct locale value
            pay.AddRequestData("vnp_OrderInfo", $"Customer: {model.Name} \n Order Description: {model.OrderDescription} \n Total Money {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", orderId);

            var paymentUrl = pay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "M4LMA4W3MH8B09THZG2NKJ1YD5YXFE1O"); // Direct BaseUrl and HashSecret

            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, "M4LMA4W3MH8B09THZG2NKJ1YD5YXFE1O"); // Direct HashSecret

            return response;
        }
    }
}
