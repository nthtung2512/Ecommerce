namespace MealMate.BLL.Dtos.Shipper
{
    public class ShipperDto
    {
        public required string SAddress { get; init; }
        public required string SFName { get; init; }
        public required string SLName { get; init; }
        public required string SPhoneNo { get; init; }
        public required string SEmail { get; init; }
        public required int VehicleCapacity { get; init; }
    }
}
