using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories.CartRedis;
using MealMate.DAL.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.Repositories.CartRedis
{
    internal class CartRepository : ICartRepository
    {
        private readonly MealMateDbContext _context;

        public CartRepository(MealMateDbContext context)
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

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartAsync(Guid customerId)
        {
            _context.CartItems.RemoveRange(_context.CartItems.Where(ci => ci.CustomerId == customerId));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(Guid cartItemId)
        {
            var temp = _context.CartItems.Find(cartItemId) ?? throw new EntityNotFoundException("Cart item not found.");
            _context.CartItems.Remove(temp);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetCartAsync(Guid customerId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CustomerId == customerId)
                .ToListAsync();

            if (cartItems.Count == 0)
                return [];

            return cartItems;
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
