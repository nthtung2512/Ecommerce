using Microsoft.AspNetCore.Identity;

namespace MealMate.DAL.IRepositories.auth
{
    public interface IIdentityRepository<TEntity> : IDisposable
    where TEntity : IdentityUser<Guid>
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity?> GetAsync(Guid id);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
