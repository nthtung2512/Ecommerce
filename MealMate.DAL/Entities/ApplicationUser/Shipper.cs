using MealMate.DAL.Entities.Products;

namespace MealMate.DAL.Entities.ApplicationUser
{
    public class Shipper() : ApplicationUser()
    {
        public int VehicleCapacity { get; set; } = 0;
        public ICollection<Bill> Bills { get; } = [];
    }
}
