namespace MealMate.DAL.Entities.Promotion
{
    public class PromoteCustomer
    {
        public Guid CustomerId { get; set; }
        public Guid PromotionId { get; init; }
        public required CustomerPromotion CustomerPromotion { get; init; }
    }
}
