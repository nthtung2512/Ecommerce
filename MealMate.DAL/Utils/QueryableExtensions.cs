namespace MealMate.DAL.Utils
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IsNotDeleted<T>(this IQueryable<T> queryable)
            where T : IDeletableEntity
        {
            return queryable.Where(entity => entity.IsDeleted == false);
        }
    }
}
