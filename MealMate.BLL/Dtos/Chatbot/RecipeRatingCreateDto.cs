namespace MealMate.BLL.Dtos.Chatbot
{
    public class RecipeRatingCreateDto
    {
        public required Guid CustomerId { get; set; }
        public required Guid RecipeId { get; set; }
        public required decimal Rating { get; set; }
    }
}
