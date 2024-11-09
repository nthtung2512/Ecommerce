namespace MealMate.BLL.Dtos.Customer
{
    public class CustomerCreationDto
    {
        public string? Address { get; init; }
        public required string FName { get; init; }
        public required string LName { get; init; }
        public required string CPhone { get; init; }
        public required string CEmail { get; init; }
        public required string Password { get; init; }
    }
}
