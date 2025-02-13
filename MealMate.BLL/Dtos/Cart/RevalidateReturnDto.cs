namespace MealMate.BLL.Dtos.Cart
{
    public class RevalidateReturnDto
    {
        public CartReturnDto Cart { get; set; }
        public List<ProductsNotAvailableDto> ProductsNotAvailable { get; set; } = [];
        public List<OutOfStockCartItemDto> OutOfStockCartItem { get; set; } = [];
        public List<ProductUpdatedDto> ProductUpdated { get; set; } = [];
    }
}
