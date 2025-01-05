using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace MealMate.DAL.Repositories.UnitOfWork
{
    // This class is used for begin and commit transaction to ensure consistency in the database
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MealMateDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(MealMateDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
