namespace MealMate.BLL.Dtos.Chatbot
{
    public class RecipeReturnDto
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required List<string> Tags { get; set; }
        public required List<IngredientReturnDto> Ingredients { get; set; }
        public required string Instructions { get; set; }
        public required string Summary { get; set; }
        public required int HealthScore { get; set; }
        public required int ReadyInMinutes { get; set; }
        public required int Servings { get; set; }
        public required string Image { get; set; }
        public required decimal AverageRating { get; set; }
    }
}
