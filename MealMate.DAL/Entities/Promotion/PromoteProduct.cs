using MealMate.DAL.Entities.Transactions;

namespace MealMate.DAL.Entities.Promotion
{
    public class PromoteProduct
    {
        public Guid ProductId { get; init; }
        public required Product Product { get; init; }

        public Guid PromotionId { get; init; }
        public required ProductPromotion ProductPromotion { get; init; }
    }
}
