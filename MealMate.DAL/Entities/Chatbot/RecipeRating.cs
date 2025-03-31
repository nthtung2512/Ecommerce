using FluentValidation;
using MealMate.Base;

namespace MealMate.DAL.Entities.Chatbot
{
    public class RecipeRating
    {
        public required Guid CustomerId { get; set; }
        public required Guid RecipeId { get; set; }
        public required decimal Rating { get; set; }
        public DateTime Timestamp { get; set; } = Constants.Now;
    }

    internal class RecipeRatingValidator : AbstractValidator<RecipeRating>
    {
        public RecipeRatingValidator()
        {
            RuleFor(r => r.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.")
                .Must(rating => rating * 2 == Math.Round(rating * 2))
                .WithMessage("Rating must be in increments of 0.5.");
        }
    }
}
