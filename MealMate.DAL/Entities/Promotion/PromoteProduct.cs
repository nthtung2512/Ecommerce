using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealMate.DAL.Entities.Promotion
{
    public class PromoteProduct : IDeletableEntity
    {
        [Key, Column(Order = 0)]
        public Guid ProductId { get; init; }
        public required Product Product { get; init; }

        [Key, Column(Order = 1)]
        public Guid PromotionId { get; init; }
        public required ProductPromotion ProductPromotion { get; init; }
        public bool IsDeleted { get; set; }
    }
}
