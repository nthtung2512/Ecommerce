namespace MealMate.BLL.Dtos.Chatbot
{
    public class RecipeRatingReturnDto
    {
        public required Guid CustomerId { get; set; }
        public required Guid RecipeId { get; set; }
        public required decimal Rating { get; set; }
        public required DateTime Timestamp { get; set; }
    }
}
