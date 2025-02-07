namespace MealMate.BLL.Dtos.Bills
{
    public class CancelOrderDto
    {
        public required BillDto Bill { get; set; }
        public required Guid OldShipperId { get; set; }
    }
}
