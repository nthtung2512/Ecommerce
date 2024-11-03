using MealMate.DAL.Utils.EFCore;

namespace MealMate.DAL.IRepositories
{
    public interface IRepository<TEntity, TKey> : IDisposable
        where TEntity : IEntity<TKey>
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity?> GetAsync(TKey id);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
