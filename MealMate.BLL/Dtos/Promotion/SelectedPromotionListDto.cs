namespace MealMate.BLL.Dtos.Promotion
{
    public class SelectedPromotionListDto
    {
        public Guid CustomerId { get; set; }
        public List<CustomerPromotionDto> CustomerPromotions { get; set; } = [];
    }
}
