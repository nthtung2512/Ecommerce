using MealMate.DAL.Utils.EFCore;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Entities.Promotion
{
    public abstract class Promotion(Guid id) : Entity<Guid>(id)
    {
        [Precision(3, 2)]
        public decimal Discount { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
    }
}
