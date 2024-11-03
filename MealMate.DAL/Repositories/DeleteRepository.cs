using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils;
using MealMate.DAL.Utils.EFCore;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class DeleteRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    {
        protected readonly MealMateDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public DeleteRepository(MealMateDbContext context)
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

        public virtual async Task<TEntity?> GetAsync(TKey id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity is IDeletableEntity deletableEntity)
            {
                deletableEntity.IsDeleted = true;
                _context.Entry(deletableEntity).State = EntityState.Modified;
            }
            else
            {
                _dbSet.Remove(entity);
            }

            await _context.SaveChangesAsync();
        }
    }
}
