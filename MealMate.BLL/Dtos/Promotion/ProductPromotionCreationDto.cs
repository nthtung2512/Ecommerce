namespace MealMate.BLL.Dtos.Promotion
{
    public class ProductPromotionCreationDto : PromotionCreationDto
    {
        public required ICollection<Guid> ProductIdList { get; init; }
    }
}
