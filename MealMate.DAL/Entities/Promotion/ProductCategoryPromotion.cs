namespace MealMate.DAL.Entities.Promotion
{
    public class ProductCategoryPromotion(Guid id) : Promotion(id)
    {
        public required string Category { get; set; }
        public ICollection<PromoteCategory> PromoteProductCategories { get; } = [];
    }
}
