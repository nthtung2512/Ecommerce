using MealMate.BLL.IServices.Payment;
using MealMate.DAL.Entities.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace MealMate.BLL.Services.Payment
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;
        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
        {
            model.OrderInformation = "Customer: " + model.FullName + "\n" + "Content: " + model.OrderInformation;

            // Assuming model.Amount is in USD, convert to VND
            decimal amountInUSD = (decimal)Math.Round(model.Amount, 2); // Example: 12.3
            decimal exchangeRate = 25000; // Example exchange rate: 1 USD = 25,000 VND
            long amountInVND = (long)Math.Round(amountInUSD * exchangeRate);

            var rawData =
                $"partnerCode=MOMO" +
                $"&accessKey=F8BBA842ECF85" +
                $"&requestId={model.OrderId}" +
                $"&amount={amountInVND}" +
                $"&orderId={model.OrderId}" +
                $"&orderInfo={model.OrderInformation}" +
                $"&returnUrl=http://localhost:5173/Checkout/PaymentCallBack" +
                $"&notifyUrl=http://localhost:5173/Checkout/PaymentCallBack" +
                $"&extraData=";

            // Encrypt raw data when transacting
            var signature = ComputeHmacSha256(rawData, "K951B6PE1waDMi640xX08PD3vg6EkVlz");

            // Client request an URL to do the transaction
            var client = new RestClient("https://test-payment.momo.vn/gw_payment/transactionProcessor");
            var request = new RestRequest() { Method = Method.Post };

            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                accessKey = "F8BBA842ECF85",
                partnerCode = "MOMO",
                requestType = "captureMoMoWallet", // or payWithATM
                notifyUrl = "http://localhost:5173/Checkout/PaymentCallBack",
                returnUrl = "http://localhost:5173/Checkout/PaymentCallBack",
                orderId = model.OrderId,
                amount = amountInVND.ToString(),
                orderInfo = model.OrderInformation,
                requestId = model.OrderId,
                extraData = "",
                signature
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);
            // Send the request to the server, including the MoMo API Url and request data
            var response = await client.ExecuteAsync(request);

            // Convert json response to object
            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content!)!;
        }



        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            // Collection data collected from React client
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;
            var fullName = collection.First(s => s.Key == "fullName").Value;

            return new MomoExecuteResponseModel()
            {
                FullName = string.IsNullOrEmpty(fullName.ToString()) ? string.Empty : fullName.ToString(),
                Amount = string.IsNullOrEmpty(amount.ToString()) ? "0" : amount.ToString(),
                OrderId = string.IsNullOrEmpty(orderId.ToString()) ? string.Empty : orderId.ToString(),
                OrderInfo = string.IsNullOrEmpty(orderInfo.ToString()) ? string.Empty : orderInfo.ToString()
            };

        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }

    }
}
