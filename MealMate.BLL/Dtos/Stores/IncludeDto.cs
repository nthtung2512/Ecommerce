using MealMate.BLL.Dtos.Product;

namespace MealMate.BLL.Dtos.Stores
{
    public class IncludeDto
    {
        public required Guid TransactionID { get; init; }
        public required Guid ProductID { get; init; }
        public required ProductCreationDto Product { get; init; }
        public required int NumberOfProductInBill { get; init; }
        public required double SubTotal { get; init; }
    }
}
