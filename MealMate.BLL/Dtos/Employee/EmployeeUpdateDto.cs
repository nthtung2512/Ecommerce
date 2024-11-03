namespace MealMate.BLL.Dtos.Employee
{
    public class EmployeeUpdateDto
    {
        public string? FirstName { get; init; }
        public double? Salary { get; init; }
        public string? LastName { get; init; }
        public string? Address { get; init; }
        public string? Phone { get; init; }

        public void Deconstruct(out string? firstName, out double? salary, out string? lastName, out string? address, out string? phone)
        {
            firstName = FirstName;
            salary = Salary;
            lastName = LastName;
            address = Address;
            phone = Phone;
        }
    }
}
