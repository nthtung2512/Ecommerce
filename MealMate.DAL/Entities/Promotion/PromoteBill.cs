using MealMate.DAL.Entities.Products;

namespace MealMate.DAL.Entities.Promotion
{
    public class PromoteBill
    {
        public Guid TransactionId { get; init; }
        public required Bill Bill { get; init; }

        public Guid PromotionId { get; init; }
        public required BillPromotion BillPromotion { get; init; }
    }
}
