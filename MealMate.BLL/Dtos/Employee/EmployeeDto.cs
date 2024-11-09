using MealMate.BLL.Dtos.ApplicationUser;

namespace MealMate.BLL.Dtos.Employee
{
    public class EmployeeDto : ApplicationUserDto
    {
        public required double Salary { get; init; }
        public required Guid StoreID { get; init; }
    }
}
