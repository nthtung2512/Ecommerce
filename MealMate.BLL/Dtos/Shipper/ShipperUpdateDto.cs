namespace MealMate.BLL.Dtos.Shipper
{
    public class ShipperUpdateDto
    {
        public int? VehicleCapacity { get; init; }
        public string? Status { get; init; }
        public string? SAddress { get; init; }
        public string? SFName { get; init; }
        public string? SLName { get; init; }
        public List<string>? SPhoneNo { get; init; }
        public List<string>? SArea { get; init; }

        public void Deconstruct(out int? vehicleCapacity, out string? status, out string? sAddress, out string? sFName, out string? sLName, out List<string>? sPhoneNo, out List<string>? sArea)
        {
            vehicleCapacity = VehicleCapacity;
            status = Status;
            sAddress = SAddress;
            sFName = SFName;
            sLName = SLName;
            sPhoneNo = SPhoneNo;
            sArea = SArea;
        }
    }
}
