using MealMate.DAL.Entities.ApplicationUser;

namespace MealMate.DAL.IRepositories.auth
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);
    }
}
