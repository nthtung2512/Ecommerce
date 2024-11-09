using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Promotion;

namespace MealMate.DAL.Entities.ApplicationUser
{
    public class Customer() : ApplicationUser()
    {
        public decimal TotalMoneySpent { get; set; } = 0.00m;
        public int FortuneChance { get; set; } = 0;
        public ICollection<PromoteCustomer> PromoteCustomers { get; } = [];
        public ICollection<Bill> Bills { get; } = [];
    }
}
