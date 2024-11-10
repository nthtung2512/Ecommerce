namespace MealMate.DAL.Entities.Payment
{
    // Used to create new transaction, same as Momo API in appsettings.json
    public class MomoOptionModel
    {
        public required string MomoApiUrl { get; set; }
        public required string SecretKey { get; set; }
        public required string AccessKey { get; set; }
        public required string ReturnUrl { get; set; }
        public required string NotifyUrl { get; set; }
        public required string PartnerCode { get; set; }
        public required string RequestType { get; set; }
    }
}
