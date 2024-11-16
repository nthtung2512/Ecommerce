namespace MealMate.DAL.Entities.Payment.VNPay
{
    public class PaymentInformationModel
    {
        public required string OrderType { get; set; }
        public required double Amount { get; set; }
        public required string OrderDescription { get; set; }
        public required string Name { get; set; }
    }

}
