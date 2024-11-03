using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealMate.DAL.Entities.Stores
{
    // Number of product at store
    public class AT : IDeletableEntity
    {
        [Key, Column(Order = 0)]
        public Guid ProductID { get; init; }
        public required Product Product { get; init; }

        [Key, Column(Order = 1)]
        public Guid StoreID { get; init; }
        public required Store Store { get; init; }
        public required int NumberAtStore { get; set; }
        public bool IsDeleted { get; set; }
    }
}
