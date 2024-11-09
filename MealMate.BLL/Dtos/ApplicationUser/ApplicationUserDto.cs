namespace MealMate.BLL.Dtos.ApplicationUser
{
    public class ApplicationUserDto
    {
        public required Guid Id { get; init; }
        public string Address { get; init; } = string.Empty;
        public required string FName { get; init; }
        public required string LName { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Email { get; init; }
    }
}
