namespace MealMate.BLL.Dtos.Cart
{
    public class ProductsNotAvailableDto
    {
        public required Guid ProductId { get; set; }
        public required string StoreName { get; set; }
        public required string ProductName { get; set; }
        public required string ProductImage { get; set; }
    }
}
