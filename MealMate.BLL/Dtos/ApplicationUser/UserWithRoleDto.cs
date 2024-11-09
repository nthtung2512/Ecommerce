namespace MealMate.BLL.Dtos.ApplicationUser
{
    public class UserWithRoleDto
    {
        public required ApplicationUserDto User { get; init; }
        public required string Role { get; init; }
    }
}
