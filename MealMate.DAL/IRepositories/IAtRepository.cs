using MealMate.DAL.Entities.Stores;

namespace MealMate.DAL.IRepositories
{
    public interface IAtRepository
    {
        Task<AT?> GetAtByProductIDAndStoreIDAsync(Guid productID, Guid storeID);
        Task<List<AT>> GetAtByProductIDAsync(Guid productID);
        Task CreateAsync(AT at);
        Task UpdateAsync(AT updateData);
    }
}
