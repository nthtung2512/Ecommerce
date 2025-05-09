using MealMate.DAL.Entities.Stores;

namespace MealMate.DAL.IRepositories
{
    public interface IAtRepository
    {
        Task<AT?> GetAtByProductIDAndStoreIDAsync(Guid productID, Guid storeID);
        Task<List<AT>> GetAtByProductIDAsync(Guid productID);
        Task<List<AT>> GetAtByStoreIdAsync(Guid storeId);
        Task<List<AT>> GetAtForProductsAsync(List<Guid> productIds, Guid storeId);
        Task<List<Store>> GetStoresByProductIdAsync(Guid productId);
        Task CreateAsync(AT at);
        Task UpdateAsync(AT updateData);
        Task DeleteAsync(AT deleteData);
    }
}
