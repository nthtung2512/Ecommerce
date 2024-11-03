namespace MealMate.DAL.Entities.Promotion
{
    public class ProductPromotion(Guid id) : Promotion(id)
    {
        public ICollection<PromoteProduct> PromoteProducts { get; } = [];
    }
}
