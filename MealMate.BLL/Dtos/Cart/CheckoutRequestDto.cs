using MealMate.BLL.Dtos.Promotion;

namespace MealMate.BLL.Dtos.Cart
{
    public class CheckoutRequestDto
    {
        public CartReturnDto Cart { get; set; }
        public List<CustomerPromotionDto> Promotions { get; set; }
    }
}
