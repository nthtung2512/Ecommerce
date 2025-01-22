using MealMate.BLL.Dtos.Stores;

namespace MealMate.BLL.Dtos.Bills
{
    public class BillCreationDto
    {
        public required string PaymentMethod { get; init; }
        public required DateTime DateAndTime { get; init; }
        public required Guid CustomerID { get; init; }
        public required Guid StoreID { get; init; }
        public required ICollection<IncludeCreationDto> Includes { get; init; }
        public required double TotalPrice { get; init; }
        public required string ShippingAddress { get; init; }
        public required int TotalWeight { get; init; }
    }
}
