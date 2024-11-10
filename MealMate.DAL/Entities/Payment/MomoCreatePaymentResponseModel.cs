namespace MealMate.DAL.Entities.Payment
{
    public class MomoCreatePaymentResponseModel
    {
        public required string RequestId { get; set; }
        public required int ErrorCode { get; set; }
        public required string OrderId { get; set; }
        public required string Message { get; set; }
        public required string LocalMessage { get; set; }
        public required string RequestType { get; set; }
        public required string PayUrl { get; set; }
        public required string Signature { get; set; }
        public required string QrCodeUrl { get; set; }
        public required string Deeplink { get; set; }
        public required string DeeplinkWebInApp { get; set; }
    }

}
