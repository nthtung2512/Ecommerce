namespace MealMate.BLL.Dtos.Product
{
    public class ProductRestockDto
    {
        public required Guid ProductID { get; init; }
        public required Guid StoreID { get; init; }
        public required int NumberAtStore { get; init; }
        public required string ProductName { get; init; }
        public required double Price { get; init; }
    }
}
