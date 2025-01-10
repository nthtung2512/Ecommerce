using MealMate.BLL.Dtos.Bills;

namespace MealMate.BLL.Dtos.Cart
{
    public class CartReturnDto
    {
        public Guid CustomerId { get; set; }
        public List<CartItemReturnDto> CartItems { get; set; } = [];
        public double TotalPrice { get; set; }
    }
}
