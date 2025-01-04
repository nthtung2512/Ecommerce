namespace MealMate.BLL.Dtos.Bills
{
    public class CartItemReturnDto
    {
        public Guid CartId { get; set; }
        public Guid ProductID { get; set; }
        public required string PName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Guid StoreID { get; set; }
        public required string StoreName { get; set; }
        public decimal Discount { get; set; }
        public double DiscountedPrice { get; set; }
        public int Weight { get; set; }
        public string ImageURL { get; set; } = string.Empty;
    }
}
