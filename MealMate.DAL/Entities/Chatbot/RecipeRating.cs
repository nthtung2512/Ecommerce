namespace MealMate.DAL.Entities.Chatbot
{
    public class RecipeRating
    {
        public Guid CustomerId { get; set; }
        public Guid RecipeId { get; set; }

        private decimal _rating;

        public decimal Rating
        {
            get => _rating;
            set
            {
                // Ensure Rating is between 1 and 5 with precision of 0.5
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(Rating), "Rating must be between 1 and 5.");
                if (value * 2 != Math.Round(value * 2))
                    throw new ArgumentException("Rating must be in increments of 0.5.");
                _rating = value;
            }
        }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
