namespace MealMate.BLL.Dtos.Shipper
{
    public class ShipperCreationDto
    {
        public required string Address { get; init; }
        public required string FName { get; init; }
        public required string LName { get; init; }
        public required string SPhoneNo { get; init; }
        public required string SEmail { get; init; }
        public required string Password { get; init; }
    }
}
