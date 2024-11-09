namespace MealMate.BLL.Dtos.Promotion
{
    public class CustomerPromotionCreationDto : PromotionCreationDto
    {
        public required Guid ProductId { get; init; }
    }
}
