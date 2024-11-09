namespace MealMate.BLL.Dtos.Customer
{
    public class CustomerUpdateDto
    {
        public string? Address { get; init; }
        public string? FName { get; init; }
        public string? LName { get; init; }
        public string? CPhone { get; init; }
        public void Deconstruct(out string? cAddress, out string? cFName, out string? cLName, out string? cPhone)
        {
            cAddress = Address;
            cFName = FName;
            cLName = LName;
            cPhone = CPhone;
        }
    }
}
