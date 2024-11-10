namespace MealMate.DAL.Entities.Payment
{
    public class OrderInfoModel
    {
        public required string FullName { get; set; }
        public required string OrderId { get; set; }
        public required string OrderInformation { get; set; }
        public required double Amount { get; set; }
    }
}
