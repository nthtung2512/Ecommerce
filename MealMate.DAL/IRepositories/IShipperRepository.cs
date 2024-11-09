using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.IRepositories.auth;

namespace MealMate.DAL.IRepositories
{
    public interface IShipperRepository : IIdentityRepository<Shipper>
    {
        Task<List<Shipper>> GetListAsync();
        Task<Shipper?> GetShipperByPhoneNumberAsync(string phoneno);
    }
}
