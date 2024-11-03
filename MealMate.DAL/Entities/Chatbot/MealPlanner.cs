using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealMate.DAL.Entities.Chatbot
{
    public class MealPlanner
    {
        [Key, Column(Order = 0)]
        public Guid RecipeID { get; set; }
        [Key, Column(Order = 1)]
        public Guid CustomerID { get; set; }
        public required string MealType { get; set; }
        public required DateTime PlanDate { get; set; }
    }
}
