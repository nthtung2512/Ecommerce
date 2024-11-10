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
            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInformation = "Customer: " + model.FullName + "\n" + "Content: " + model.OrderInformation;
            var rawData =
                $"partnerCode={_options.Value.PartnerCode}" +
                $"&accesskey={_options.Value.AccessKey}" +
                $"&requestId={model.OrderId}" +
                $"&amount={model.Amount}" +
                $"&orderId={model.OrderId}" +
                $"&orderInfo={model.OrderInformation}" +
                $"&returnUrl={_options.Value.ReturnUrl}" +
                $"&notifyUrl={_options.Value.NotifyUrl}" +
                $"&extraData=";

            // Encrypt raw data when transacting
            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            // Client request an URL to do the transaction
            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };

            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                accesskey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                amount = model.Amount.ToString(),
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

            return new MomoExecuteResponseModel()
            {
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
