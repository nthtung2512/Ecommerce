﻿using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.Dtos.Cart;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.Dtos.Stores;
using MealMate.BLL.IServices.Hubs;
using MealMate.BLL.IServices.Redis;
using MealMate.BLL.IServices.Utility;
using MealMate.BLL.Services.Hubs;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.IRepositories;
using MealMate.DAL.IRepositories.CartRedis;
using MealMate.DAL.Utils.Exceptions;
using Microsoft.AspNetCore.SignalR;

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
        private readonly IHubContext<ProductHub, IProductHubClient> _productHubContext;

        public ReserveCartCacheService(IRedisCacheService redisCacheService, IAtRepository atRepository, IMapProductService productAppService, IProductRepository productRepository, IReserveCartItemCacheService reserveCartItemCacheService, ICartRepository cartRepository, IHubContext<ProductHub, IProductHubClient> productHubContext)
        {
            _redisCacheService = redisCacheService;
            _atRepository = atRepository;
            _mapProductAppService = productAppService;
            _productRepository = productRepository;
            _reserveCartItemCacheService = reserveCartItemCacheService;
            _cartRepository = cartRepository;
            _productHubContext = productHubContext;
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

        public async Task CheckoutCartAsync(CartReturnDto cart, List<CustomerPromotionDto> customerPromotions)
        {
            var key = $"ReserveCart:{cart.CustomerId}";

            foreach (var item in cart.CartItems)
            {
                await _reserveCartItemCacheService.AddCartItemToRedis(item);
            }

            var promotionKey = $"SelectedPromotion:{cart.CustomerId}";
            var currentCustomerPromotions = await _redisCacheService.GetDataAsync<SelectedPromotionListDto>(promotionKey);

            if (currentCustomerPromotions != null)
            {
                var redisDatabase = _redisCacheService.GetDatabase();

                // Get the current TTL
                var currentTTL = await redisDatabase.KeyTimeToLiveAsync(promotionKey);

                // Add 15 minutes to the existing TTL if it exists
                if (currentTTL.HasValue)
                {
                    var newTTL = currentTTL.Value + TimeSpan.FromMinutes(15);
                    await redisDatabase.KeyExpireAsync(promotionKey, newTTL);
                }
                else
                {
                    // If the key doesn't have an expiry, set it to 15 minutes from now
                    await redisDatabase.KeyExpireAsync(promotionKey, TimeSpan.FromMinutes(15));
                }
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

        public async Task<CheckoutRequestDto> GetCheckoutDataAsync(Guid customerId)
        {
            var key = $"ReserveCart:{customerId}";
            var reserveCart = await _redisCacheService.GetDataAsync<CartReturnDto>(key) ?? throw new EntityNotFoundException("No cart found");

            var promotionKey = $"SelectedPromotion:{customerId}";
            var customerPromotions = await _redisCacheService.GetDataAsync<SelectedPromotionListDto>(promotionKey);

            return new CheckoutRequestDto
            {
                Cart = reserveCart,
                Promotions = customerPromotions?.CustomerPromotions ?? []
            };
        }

        public async Task ReduceStockOnCheckoutAsync(CartReturnDto cart)
        {
            foreach (var item in cart.CartItems)
            {
                await _productHubContext.Clients.Group($"{item.ProductID}_{item.StoreID}").ReceiveChangeStock(item.ProductID, item.Quantity);
            }
        }

        public async Task AddStockOnNoPurchaseAsync(CartReturnDto cart)
        {
            foreach (var item in cart.CartItems)
            {
                await _productHubContext.Clients.Group($"{item.ProductID}_{item.StoreID}").ReceiveChangeStock(item.ProductID, -item.Quantity);
            }
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
            await AddStockOnNoPurchaseAsync(reserveCart);
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

        public async Task<int?> GetCartTimerAsync(Guid customerId)
        {
            var key = $"ReserveCart:{customerId}";
            var redisDatabase = _redisCacheService.GetDatabase();

            // Get the current TTL
            var timer = await redisDatabase.KeyTimeToLiveAsync(key);
            if (timer.HasValue)
            {
                return (int)timer.Value.TotalSeconds;
            }
            else
            {
                return null;
            }
        }
    }
}
