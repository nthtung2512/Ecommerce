namespace MealMate.DAL.Entities.Promotion
{
    public class CustomerPromotion(Guid id) : Promotion(id)
    {
        public required Guid ProductId { get; init; }
    }
}
