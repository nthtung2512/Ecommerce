namespace MealMate.DAL.Entities.ApplicationUser
{
    public class Shipper(Guid id) : ApplicationUser(id)
    {
        public int VehicleCapacity { get; set; } = 0;
    }
}
