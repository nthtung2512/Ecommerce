namespace MealMate.BLL.Dtos.Promotion
{
    public class BillPromotionCreationDto : PromotionCreationDto
    {
        public required int ApplyPrice { get; init; }
        public required int PromotionChance { get; init; }
    }
}
