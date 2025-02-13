namespace MealMate.BLL.Dtos.Cart
{
    public class ProductUpdatedDto
    {
        public required Guid ProductID { get; set; }
        public required string StoreName { get; set; }
        public required string ProductName { get; set; }
        public required string ProductImage { get; set; }
        public (string, string)? NameUpdate { get; set; }
        public (decimal, decimal)? DiscountUpdate { get; set; }
        public (double, double)? PriceUpdate { get; set; }
        public (int, int)? WeightUpdate { get; set; }
    }
}
