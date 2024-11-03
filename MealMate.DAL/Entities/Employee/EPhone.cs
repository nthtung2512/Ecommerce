using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealMate.DAL.Entities.Employee
{
    public class EPhone
    {
        [Key, Column(Order = 0)]
        public Guid ManagerId { get; set; }

        [Key, Column(Order = 1)]
        public required string EPhoneNumber { get; set; }
    }
}
