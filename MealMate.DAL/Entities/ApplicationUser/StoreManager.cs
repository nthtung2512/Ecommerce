namespace MealMate.DAL.Entities.ApplicationUser
{
    public class StoreManager(Guid id) : ApplicationUser(id)
    {
        public required double Salary { get; set; }
        public required Guid StoreId { get; set; }
    }
}
