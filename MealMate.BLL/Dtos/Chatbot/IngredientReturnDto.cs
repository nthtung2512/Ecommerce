namespace MealMate.BLL.Dtos.Chatbot
{
    public class IngredientReturnDto
    {
        public required Guid RecipeId { get; set; }
        public required string Name { get; set; }
        public required double Amount { get; set; }
        public required string Unit { get; set; }
        public required string Original { get; set; }
        public required string NameClean { get; set; }
    }
}
