namespace MealMate.DAL.Entities.ApplicationUser
{
    public class StoreManager() : ApplicationUser()
    {
        public required double Salary { get; set; }
        public required Guid StoreId { get; set; }
    }
}
