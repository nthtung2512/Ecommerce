using MealMate.BLL.Dtos.Stores;

namespace MealMate.BLL.Dtos.Bills
{
    public class BillCreationDto
    {
        public required string PaymentMethod { get; init; }
        public required DateTime DateAndTime { get; init; }
        public required Guid CustomerID { get; init; }
        public required Guid StoreID { get; init; }
        public ICollection<IncludeCreationDto> Includes { get; } = [];
        public required double TotalPrice { get; init; }
        public required int TotalWeight { get; init; }
    }
}
