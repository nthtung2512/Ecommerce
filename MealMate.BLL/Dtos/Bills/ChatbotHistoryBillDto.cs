using MealMate.BLL.Dtos.Stores;
using MealMate.DAL.Utils.Enum;

namespace MealMate.BLL.Dtos.Bills
{
    public class ChatbotHistoryBillDto
    {
        public required Guid TransactionId { get; init; }
        public required Guid CustomerID { get; init; }
        public required DateTime DateAndTime { get; init; }
        public required double TotalPrice { get; init; }
        public ICollection<IncludeDto> Includes { get; set; } = [];
    }
}
