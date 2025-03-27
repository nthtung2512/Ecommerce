using MealMate.DAL.Entities.Products;

namespace MealMate.DAL.Entities.ApplicationUser
{
    public class Shipper() : ApplicationUser()
    {
        public ICollection<Bill> Bills { get; } = [];
    }
}
