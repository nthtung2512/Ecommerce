namespace MealMate.BLL.Dtos.Product
{
    public class ProductCreationDto
    {
        public Guid ProductId { get; init; }
        public required string Image { get; init; }
        public required string Consistency { get; init; }
        public required string Name { get; init; }
        public required string NameClean { get; init; }
        public required string OriginalName { get; init; }
        public required int Amount { get; init; }
        public string Unit { get; init; } = string.Empty;
        public required double Price { get; init; }
        public required string Aisle { get; init; }
        public string Description { get; init; } = "Fresh food from MealMate";
    }
}
