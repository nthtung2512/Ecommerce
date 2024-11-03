using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.Entities.Stores;
using MealMate.DAL.Utils;
using MealMate.DAL.Utils.EFCore;
using MealMate.DAL.Utils.Enum;

namespace MealMate.DAL.Entities.Products
{
    public class Bill(Guid id) : Entity<Guid>(id), IDeletableEntity
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime DateAndTime { get; set; }
        public required Guid CustomerID { get; init; }
        public Customer? Customer { get; init; }
        public required Guid StoreID { get; init; }
        public Store? Store { get; init; }
        public Guid ShipperID { get; set; }
        public Shipper? Shipper { get; set; }
        public List<Include> Includes { get; } = [];
        public decimal TotalPrice { get; set; }
        public double TotalWeight { get; set; }
        public required DeliveryStatus DeliveryStatus { get; set; }
        public bool IsDeleted { get; set; }
    }

}
