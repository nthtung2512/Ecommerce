namespace MealMate.BLL.Dtos.Stores
{
    public class IncludeCreationDto
    {
        public required Guid ProductID { get; init; }

        public required int NumberOfProductInBill { get; init; }
        public required double SubTotal { get; init; }
    }
}
