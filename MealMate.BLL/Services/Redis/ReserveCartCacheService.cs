using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.Dtos.Cart;
using MealMate.BLL.Dtos.Stores;
using MealMate.BLL.IServices.Redis;
using MealMate.BLL.IServices.Utility;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.IRepositories;
using MealMate.DAL.IRepositories.CartRedis;
using MealMate.DAL.Utils.Exceptions;

namespace MealMate.BLL.Services.Redis
{
    internal class ReserveCartCacheService : IReserveCartCacheService
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly IAtRepository _atRepository;
        private readonly IProductRepository _productRepository;
        private readonly IReserveCartItemCacheService _reserveCartItemCacheService;
        private readonly IMapProductService _mapProductAppService;
        private readonly ICartRepository _cartRepository;

        public ReserveCartCacheService(IRedisCacheService redisCacheService, IAtRepository atRepository, IMapProductService productAppService, IProductRepository productRepository, IReserveCartItemCacheService reserveCartItemCacheService, ICartRepository cartRepository)
        {
            _redisCacheService = redisCacheService;
            _atRepository = atRepository;
            _mapProductAppService = productAppService;
            _productRepository = productRepository;
            _reserveCartItemCacheService = reserveCartItemCacheService;
            _cartRepository = cartRepository;
        }

        public async Task HandleExpiredCartAsync(string customerId)
        {
            var key = $"Cart:{customerId}";
            // Retrieve cart (if still available)
            var cart = await _redisCacheService.GetDataAsync<CartReturnDto>(key);
            List<CartItem> cartItems = [];
            if (cart != null)
            {
                foreach (var item in cart.CartItems)
                {
                    cartItems.Add(new CartItem
                    {
                        ProductID = item.ProductID,
                        StoreID = item.StoreID,
                        Quantity = item.Quantity
                    });
                }
            }
            else
            {
                cartItems = await _cartRepository.GetCartAsync(Guid.Parse(customerId));
            }

            foreach (var item in cartItems)
            {
                var cartItemKey = $"CartItemReserve:{item.ProductID}-{item.StoreID}";

                // Retrieve the current cart item from Redis
                var cartItem = await _redisCacheService.GetDataAsync<CartItemReturnDto>(cartItemKey);

                if (cartItem != null)
                {
                    // Adjust the quantity or remove the item if no quantity is left
                    cartItem.Quantity -= item.Quantity;

                    if (cartItem.Quantity <= 0)
                    {
                        // Remove the cart item if quantity is zero or less
                        await _redisCacheService.RemoveDataAsync(cartItemKey);
                    }
                    else
                    {
                        // Update the Redis cache with the reduced quantity
                        await _redisCacheService.SetDataAsync(cartItemKey, cartItem, TimeSpan.FromMinutes(15));
                    }
                }
            }

            // Optionally log the expiration handling
            Console.WriteLine($"Handled expiration for ReserveCart:{customerId}");
        }

        public async Task CheckoutCartAsync(CartReturnDto cart)
        {
            var key = $"ReserveCart:{cart.CustomerId}";

            foreach (var item in cart.CartItems)
            {
                await _reserveCartItemCacheService.AddCartItemToRedis(item);
            }

            await _redisCacheService.SetDataAsync(key, cart, TimeSpan.FromMinutes(15));
            /* foreach (var item in cart.CartItems)
             {
                 // Add cart item to Redis for checkout - 15 minutes
                 var key = $"CartItemReserve:{cart.CustomerId}-{item.ProductID}-{item.StoreID}";
                 await _redisCacheService.SetDataAsync(key, item, TimeSpan.FromMinutes(15));

                 // Realtime update stock
                 var groupname = $"{item.ProductID}-{item.StoreID}";
                 await _productHubClient.Clients.Group(groupname).ReceiveChangeStock(item.ProductID, item.Quantity);
             }*/
        }

        public async Task RemoveReserveCartAsync(Guid customerId)
        {
            var key = $"ReserveCart:{customerId}";
            var reserveCart = await _redisCacheService.GetDataAsync<CartReturnDto>(key);
            await _redisCacheService.RemoveDataAsync(key);
            if (reserveCart == null)
            {
                return;
            }
            foreach (var item in reserveCart.CartItems)
            {
                var cartItemKey = $"CartItemReserve:{item.ProductID}-{item.StoreID}";

                // Retrieve the current cart item from Redis
                var cartItem = await _redisCacheService.GetDataAsync<CartItemReturnDto>(cartItemKey);

                if (cartItem != null)
                {
                    // Adjust the quantity or remove the item if no quantity is left
                    cartItem.Quantity -= item.Quantity;

                    if (cartItem.Quantity <= 0)
                    {
                        // Remove the cart item if quantity is zero or less
                        await _redisCacheService.RemoveDataAsync(cartItemKey);
                    }
                    else
                    {
                        // Update the Redis cache with the reduced quantity
                        await _redisCacheService.SetDataAsync(cartItemKey, cartItem, TimeSpan.FromMinutes(15));
                    }
                }
            }
            /*var keyPattern = $"CartItemReserve:{customerId}-*";
            var cartItemKeys = _redisCacheService.ScanKeys(keyPattern);
            foreach (var key in cartItemKeys)
            {
                await _redisCacheService.RemoveDataAsync(key);
            }*/
        }

        public async Task<bool> CheckIfCustomerCheckoutAsync(Guid customerId)
        {
            var key = $"ReserveCart:{customerId}";
            var reserveCart = await _redisCacheService.GetDataAsync<CartReturnDto>(key);
            if (reserveCart == null)
            {
                return false;
            }
            return true;
        }

        public async Task<int> GetQuantityFromCache(Guid productId, Guid storeId)
        {
            var item = await _reserveCartItemCacheService.GetCartItemCache(productId, storeId);
            if (item == null)
            {
                return 0;
            }
            return item.Quantity;
        }

        public async Task<ATDto> GetAtByProductIDAndStoreIDAsync(Guid productId, Guid storeId)
        {
            var productAtStore = await _atRepository.GetAtByProductIDAndStoreIDAsync(productId, storeId) ?? throw new EntityNotFoundException("Product not found at store");
            int amount = productAtStore.NumberAtStore;

            var amountInCache = await GetQuantityFromCache(productId, storeId);
            amount -= amountInCache;

            var productDto = await _mapProductAppService.MapProductDto(productAtStore.Product);
            return new ATDto
            {
                ProductID = productId,
                StoreID = storeId,
                NumberAtStore = amount,
                Product = productDto
            };
        }

        public async Task<List<ATDto>> GetAtByProductIDAsync(Guid productId)
        {
            var at = await _atRepository.GetAtByProductIDAsync(productId);
            if (at.Count == 0)
            {
                throw new EntityNotFoundException("No product found");
            }
            var atDtos = new List<ATDto>();
            foreach (var item in at)
            {
                var product = await _productRepository.GetAsync(productId) ?? throw new EntityNotFoundException("No product found");
                var productDto = await _mapProductAppService.MapProductDto(product);

                var amountInCache = await GetQuantityFromCache(productId, item.StoreID);
                item.NumberAtStore -= amountInCache;

                atDtos.Add(new ATDto
                {
                    ProductID = productId,
                    StoreID = item.StoreID,
                    NumberAtStore = item.NumberAtStore,
                    Product = productDto
                });
            }
            return atDtos;
        }

        public async Task<List<ATDto>> GetAtByStoreIdAsync(Guid storeId)
        {
            var at = await _atRepository.GetAtByStoreIdAsync(storeId);
            if (at.Count == 0)
            {
                throw new EntityNotFoundException("No product found for this store");
            }
            var atDtos = new List<ATDto>();
            foreach (var item in at)
            {
                var product = await _productRepository.GetAsync(item.ProductID) ?? throw new EntityNotFoundException("No product found");
                var productDto = await _mapProductAppService.MapProductDto(product);

                var amountInCache = await GetQuantityFromCache(item.ProductID, storeId);
                item.NumberAtStore -= amountInCache;

                atDtos.Add(new ATDto
                {
                    ProductID = item.ProductID,
                    StoreID = item.StoreID,
                    NumberAtStore = item.NumberAtStore,
                    Product = productDto
                });
            }
            return atDtos;
        }
    }
}
