namespace MealMate.DAL.Entities.ApplicationUser
{
    public class Customer(Guid id) : ApplicationUser(id)
    {
        public decimal TotalMoneySpent { get; set; } = 0.0m;
        public int FortuneChance { get; set; } = 0;
    }
}
