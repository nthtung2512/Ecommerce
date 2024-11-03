using MealMate.DAL.Entities.Products;
using MealMate.DAL.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealMate.DAL.Entities.Promotion
{
    public class PromoteBill : IDeletableEntity
    {
        [Key, Column(Order = 0)]
        public Guid TransactionId { get; init; }
        public required Bill Bill { get; init; }

        [Key, Column(Order = 1)]
        public Guid PromotionId { get; init; }
        public required BillPromotion BillPromotion { get; init; }
        public bool IsDeleted { get; set; }
    }
}
