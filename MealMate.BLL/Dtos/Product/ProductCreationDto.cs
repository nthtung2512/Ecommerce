namespace MealMate.BLL.Dtos.Product
{
    public class ProductCreationDto
    {
        public Guid ProductId { get; init; }
        public required string Category { get; init; }
        public required string Description { get; init; }
        public required string PName { get; init; }
        public required double Price { get; init; }
        public required int Weight { get; init; }
        public required string ImageURL { get; init; }
    }
}
