using Microsoft.EntityFrameworkCore;

namespace MealMate.BLL.Dtos.Product
{
    public class ProductDto
    {
        public Guid ProductID { get; init; }

        public required string Category { get; init; }

        public string? Description { get; init; }

        public required string PName { get; init; }
        public required double Price { get; init; }
        [Precision(3, 2)]
        public required decimal Discount { get; init; }
        public required double DiscountedPrice { get; init; }

        public required double Weight { get; init; }
        public required string ImageURL { get; init; }
    }
}
