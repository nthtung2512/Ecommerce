using MealMate.BLL.Dtos.ApplicationUser;

namespace MealMate.BLL.Dtos.Shipper
{
    public class ShipperDto : ApplicationUserDto
    {
        public required int VehicleCapacity { get; init; }
    }
}
