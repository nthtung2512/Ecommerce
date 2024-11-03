namespace MealMate.BLL.Dtos.Customer
{
    public class CustomerCreationDto
    {
        public Guid CustomerID { get; init; }
        public string? CAddress { get; init; }
        public required string CFName { get; init; }
        public required string CLName { get; init; }
        public required string CPhone { get; init; }
        public required string CEmail { get; init; }
        public required string Password { get; init; }
    }
}
