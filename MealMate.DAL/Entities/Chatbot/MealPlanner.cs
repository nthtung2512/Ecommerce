namespace MealMate.DAL.Entities.Chatbot
{
    public class MealPlanner
    {
        public string RecipeID { get; set; } = string.Empty;
        public Guid CustomerID { get; set; }
        public required string MealType { get; set; }
        public required DateTime PlanDate { get; set; }
    }
}
