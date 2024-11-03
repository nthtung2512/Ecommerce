namespace MealMate.DAL.Utils
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}
