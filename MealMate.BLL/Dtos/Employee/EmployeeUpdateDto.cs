namespace MealMate.BLL.Dtos.Employee
{
    public class EmployeeUpdateDto
    {
        public string? FName { get; init; }
        public double? Salary { get; init; }
        public string? LName { get; init; }
        public string? Address { get; init; }
        public string? Phone { get; init; }
        public Guid? StoreID { get; init; }

        public void Deconstruct(out string? firstName, out double? salary, out string? lastName, out string? address, out string? phone, out Guid? storeID)
        {
            firstName = FName;
            salary = Salary;
            lastName = LName;
            address = Address;
            phone = Phone;
            storeID = StoreID;
        }
    }
}
