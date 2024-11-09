using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.Utils;

namespace MealMate.DAL.Entities.Stores
{
    // Number of product at store
    public class AT : IDeletableEntity
    {
        public Guid ProductID { get; init; }
        public required Product Product { get; init; }

        public Guid StoreID { get; init; }
        public required Store Store { get; init; }
        public required int NumberAtStore { get; set; }
        public bool IsDeleted { get; set; }
    }
}
