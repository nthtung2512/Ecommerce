namespace MealMate.DAL.Entities.Promotion
{
    public class BillPromotion(Guid id) : Promotion(id)
    {
        public required int ApplyPrice { get; set; }
        public required int PromotionChance { get; set; }
        public ICollection<PromoteBill> PromoteBills { get; } = [];
    }
}
