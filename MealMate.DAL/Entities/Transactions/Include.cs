using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.Utils;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Entities.Products
{
    public class Include : IDeletableEntity
    {
        public Guid TransactionID { get; set; }

        public Guid ProductID { get; set; }

        public required Bill Transaction { get; init; }
        public required Product Product { get; init; }
        public int NumberOfProductInBill { get; set; }
        [Precision(3, 2)]
        public double SubTotal { get; set; }
        public bool IsDeleted { get; set; }
    }
}
