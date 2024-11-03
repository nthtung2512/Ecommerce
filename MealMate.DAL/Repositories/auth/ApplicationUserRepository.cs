using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories.auth;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories.auth
{
    internal class ApplicationUserRepository : IApplicationUserRepository
    {
        protected readonly MealMateDbContext _context;

        public ApplicationUserRepository(MealMateDbContext context)
        {
            _context = context;
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


        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
