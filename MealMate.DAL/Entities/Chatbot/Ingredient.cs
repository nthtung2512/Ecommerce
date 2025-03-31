namespace MealMate.DAL.Entities.Chatbot
{
    public class Ingredient
    {
        public required Guid RecipeId { get; set; }
        public required Recipe Recipe { get; set; }
        public required string Name { get; set; }
        public required double Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Original { get; set; } = string.Empty;
        public string NameClean { get; set; } = string.Empty;
    }
}
