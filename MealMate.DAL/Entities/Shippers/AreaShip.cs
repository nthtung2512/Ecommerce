using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealMate.DAL.Entities.Shippers
{
    public class AreaShip
    {
        [Key, Column(Order = 0)]
        public Guid ShipperID { get; set; }
        [Key, Column(Order = 1)]
        public required string Area { get; set; }
    }
}
