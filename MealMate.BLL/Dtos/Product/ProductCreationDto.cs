namespace MealMate.BLL.Dtos.Product
{
    public class ProductCreationDto
    {
        public required string Category { get; init; }
        public required string Description { get; init; }
        public required string PName { get; init; }
        public required double Price { get; init; }
        public required double Weight { get; init; }
        public required string ImageURL { get; init; }
    }
}
