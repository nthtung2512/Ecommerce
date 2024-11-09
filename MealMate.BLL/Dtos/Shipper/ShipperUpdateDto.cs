namespace MealMate.BLL.Dtos.Shipper
{
    public class ShipperUpdateDto
    {
        public string? Address { get; init; }
        public string? FName { get; init; }
        public string? LName { get; init; }
        public string? SPhoneNo { get; init; }
        public void Deconstruct(out string? sAddress, out string? sFName, out string? sLName, out string? sPhoneNo)
        {
            sAddress = Address;
            sFName = FName;
            sLName = LName;
            sPhoneNo = SPhoneNo;
        }
    }
}
