using MealMate.BLL.Dtos.ApplicationUser;

namespace MealMate.BLL.Dtos.Customer
{
    public class CustomerDto : ApplicationUserDto
    {
        public required decimal TotalMoneySpent { get; set; }
        public required int FortuneChance { get; set; }
    }
}
