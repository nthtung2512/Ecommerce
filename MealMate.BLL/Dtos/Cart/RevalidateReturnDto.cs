using MealMate.BLL.Dtos.Bills;

namespace MealMate.BLL.Dtos.Cart
{
    public class RevalidateReturnDto
    {
        public CartReturnDto Cart { get; set; }
        public List<CartItemReturnDto> ProductsNotAvailable { get; set; } = [];
        public List<CartItemReturnDto> OutOfStockCartItem { get; set; } = [];
        public List<CartItemReturnDto> ProductUpdated { get; set; } = [];
    }
}
