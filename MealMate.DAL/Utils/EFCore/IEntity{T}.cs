namespace MealMate.DAL.Utils.EFCore
{
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }
}
