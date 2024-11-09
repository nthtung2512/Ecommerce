using MealMate.BLL.Dtos.Stores;
using MealMate.DAL.Utils.Enum;

namespace MealMate.BLL.Dtos.Bills
{
    public class FullBillDto
    {
        public required Guid TransactionId { get; init; }
        public required Guid CustomerID { get; init; }
        public required Guid StoreID { get; init; }
        public Guid? ShipperID { get; init; }
        public required DateTime DateAndTime { get; init; }
        public required DeliveryStatus DeliveryStatus { get; init; }
        public required double TotalPrice { get; init; }
        public required int TotalWeight { get; init; }
        public ICollection<IncludeDto> Includes { get; set; } = [];
    }
}
