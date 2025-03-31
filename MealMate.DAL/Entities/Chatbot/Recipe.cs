using MealMate.DAL.Utils.EFCore;

namespace MealMate.DAL.Entities.Chatbot
{
    public class Recipe(Guid id) : Entity<Guid>(id)
    {
        public required string Title { get; set; }
        public required string Tags { get; set; }
        public List<Ingredient> Ingredients { get; } = [];
        public string Instructions { get; set; } = string.Empty;
        public required string Summary { get; set; }
        public required int HealthScore { get; set; }
        public required int ReadyInMinutes { get; set; }
        public required int Servings { get; set; }
        public string Image { get; set; } = string.Empty;
        public string[] GetListTags() => Tags.Split(';');
    }
}
