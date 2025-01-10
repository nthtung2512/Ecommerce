using MealMate.DAL.Entities.Transactions;

namespace MealMate.DAL.IRepositories.CartRedis
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetCartAsync(Guid customerId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(Guid cartItemId);
        Task DeleteCartAsync(Guid customerId);
    }
}
