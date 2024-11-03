using MealMate.DAL.Utils.EFCore;

namespace MealMate.DAL.Entities.Chatbot
{
    public class Recipe(Guid id) : Entity<Guid>(id)
    {
        public required string RecipeName { get; set; }
        public required string RecipeDescription { get; set; }
        public required string DifficultyLevel { get; set; }
        public required string RecipeIngredients { get; set; }
        public required string RecipeInstructions { get; set; }
        public string RecipeTags { get; set; } = string.Empty;
        public required string CookingTime { get; set; }
        public required string NumOfServe { get; set; }
        public string[] GetListRecipeIngredients() => RecipeIngredients.Split(';');
        public string[] GetListRecipeInstructions() => RecipeInstructions.Split(';');
    }
}
