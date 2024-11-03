using MealMate.DAL.Entities.Stores;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories
{
    internal class StoreRepository(MealMateDbContext context) : Repository<Store, Guid>(context), IStoreRepository
    {
        public async Task<List<Store>> GetAllStoresAsync()
        {
            return await Query.ToListAsync() ?? [];
        }
    }
}
