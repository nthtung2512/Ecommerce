namespace MealMate.DAL.Entities.Payment
{
    public class MomoExecuteResponseModel
    {
        // Bill id
        public required string OrderId { get; set; }
        // Total amount of money of the bill (Grand total)
        public required string Amount { get; set; }
        public required string OrderInfo { get; set; }
    }
}
