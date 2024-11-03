namespace MealMate.BLL.Dtos.Customer
{
    public class CustomerDto
    {
        public Guid CustomerID { get; init; }
        public string CAddress { get; init; } = string.Empty;
        public required string CFName { get; init; }
        public required string CLName { get; init; }
        public required string CPhone { get; init; }
        public required string CEmail { get; init; }
        public decimal TotalMoneySpent { get; set; }
    }
}
