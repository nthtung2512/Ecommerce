namespace MealMate.BLL.Dtos.Stores
{
    public class IncludeDto
    {
        public required Guid TransactionID { get; init; }
        public required Guid ProductID { get; init; }
        public required string ProductName { get; init; }
        public required double ProductPrice { get; init; }
        public required int NumberOfProductInBill { get; init; }
        public required double SubTotal { get; init; }
    }
}
