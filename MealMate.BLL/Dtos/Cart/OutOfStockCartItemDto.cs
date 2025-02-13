namespace MealMate.BLL.Dtos.Cart
{
    public class OutOfStockCartItemDto
    {
        public required Guid ProductId { get; set; }
        public required Guid StoreId { get; set; }
        public required string StoreName { get; set; }
        public required string ProductName { get; set; }
        public required int AvailableQuantity { get; set; }
        public required string ProductImage { get; set; }
    }
}
