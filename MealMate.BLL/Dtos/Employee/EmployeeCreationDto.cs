namespace MealMate.BLL.Dtos.Employee
{
    public class EmployeeCreationDto
    {
        public required string FName { get; init; }
        public required double Salary { get; init; }
        public required string LName { get; init; }
        public string? Address { get; init; }
        public required string Email { get; init; }
        public required Guid StoreID { get; init; }
        public required string EPhone { get; init; }
        public required string Password { get; init; }
    }
}
