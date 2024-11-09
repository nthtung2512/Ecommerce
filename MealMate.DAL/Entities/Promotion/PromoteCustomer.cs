using MealMate.DAL.Entities.ApplicationUser;

namespace MealMate.DAL.Entities.Promotion
{
    public class PromoteCustomer
    {
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid PromotionId { get; init; }
        public CustomerPromotion? CustomerPromotion { get; set; }
    }
}
