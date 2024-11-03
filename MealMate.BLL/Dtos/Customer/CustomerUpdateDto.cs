namespace MealMate.BLL.Dtos.Customer
{
    public class CustomerUpdateDto
    {
        public string? CAddress { get; init; }
        public string? CFName { get; init; }
        public string? CLName { get; init; }
        public string? CPhone { get; init; }
        public void Deconstruct(out string? cAddress, out string? cFName, out string? cLName, out string? cPhone)
        {
            cAddress = CAddress;
            cFName = CFName;
            cLName = CLName;
            cPhone = CPhone;
        }
    }
}
