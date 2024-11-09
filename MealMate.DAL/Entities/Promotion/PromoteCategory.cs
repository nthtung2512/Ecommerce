using MealMate.DAL.Entities.Transactions;

namespace MealMate.DAL.Entities.Promotion
{
    public class PromoteCategory
    {
        public Guid ProductId { get; init; }
        public required Product Product { get; init; }

        public Guid PromotionId { get; init; }
        public required ProductCategoryPromotion ProductCategoryPromotion { get; init; }
    }
}
