using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories.auth;
using MealMate.DAL.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories.auth
{
    public class IdentityRepository<TEntity> : IIdentityRepository<TEntity>
    where TEntity : IdentityUser<Guid>, IDeletableEntity
    {
        protected readonly MealMateDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        protected IQueryable<TEntity> Query => _dbSet.IsNotDeleted();

        public IdentityRepository(MealMateDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        #region dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        public virtual async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            return await Query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
