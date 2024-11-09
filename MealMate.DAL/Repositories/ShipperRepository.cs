using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Repositories.auth;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class ShipperRepository(MealMateDbContext context) : IdentityRepository<Shipper>(context), IShipperRepository
    {
        public async Task<List<Shipper>> GetListAsync()
        {
            return await _context.Shippers.ToListAsync();
        }

        public async Task<Shipper?> GetShipperByPhoneNumberAsync(string phoneno)
        {
            return await _context.Shippers.FirstOrDefaultAsync(s => s.PhoneNumber == phoneno);

        }
    }
}
