namespace MealMate.DAL.Entities.Chatbot
{
    public class Ingredient
    {
        public required Guid Id { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double Amount { get; set; }
    }
}
