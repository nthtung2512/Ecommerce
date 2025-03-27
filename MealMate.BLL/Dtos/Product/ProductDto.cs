using Microsoft.EntityFrameworkCore;

namespace MealMate.BLL.Dtos.Product
{
    public class ProductDto
    {
        public Guid ProductID { get; init; }

        public required string Image { get; init; }
        public required string Consistency { get; init; }
        public required string Name { get; init; }
        public required string NameClean { get; init; }
        public required string OriginalName { get; init; }
        public required int Amount { get; init; }
        public required string Unit { get; init; }
        public required double Price { get; init; }
        public required string Aisle { get; init; }
        public required string Description { get; init; }
        [Precision(3, 2)]
        public decimal Discount { get; set; }
        public double DiscountedPrice { get; set; }
    }
}
