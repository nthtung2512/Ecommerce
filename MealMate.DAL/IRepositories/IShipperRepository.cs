using MealMate.DAL.Entities.ApplicationUser;

namespace MealMate.DAL.IRepositories
{
    public interface IShipperRepository : IRepository<Shipper, Guid>
    {
        Task<List<Shipper>> GetFreeShipperByAreaAsync(string area);
        Task<Shipper?> GetPhoneNoAsync(string phoneNo);
    }
}
