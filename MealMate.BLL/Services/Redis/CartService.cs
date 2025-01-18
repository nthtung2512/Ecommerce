using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.Dtos.Cart;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices.Redis;
using MealMate.BLL.IServices.Utility;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.IRepositories;
using MealMate.DAL.IRepositories.CartRedis;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services.Redis
{
    internal class CartService : ICartService
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IProductPromotionRepository _productPromotionRepository;
        private readonly IAtRepository _atRepository;
        private readonly IMapProductService _mapProductService;
        private readonly IReserveCartCacheService _reserveCartCacheService;
        private readonly GuidGenerator _guidGenerator;

        public CartService(
            IRedisCacheService redisCacheService,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            GuidGenerator guidGenerator,
            IStoreRepository storeRepository,
            IProductPromotionRepository productPromotionRepository,
            IAtRepository atRepository,
            IMapProductService mapProductService,
            IReserveCartCacheService reserveCartCacheService)
        {
            _redisCacheService = redisCacheService;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _guidGenerator = guidGenerator;
            _storeRepository = storeRepository;
            _productPromotionRepository = productPromotionRepository;
            _atRepository = atRepository;
            _mapProductService = mapProductService;
            _reserveCartCacheService = reserveCartCacheService;
        }


        /* public async Task RevalidateCartsWithProductIdsAsync(List<Guid> productIds)
         {
             var keyPattern = "Cart:*"; // Pattern to match all cart keys
             var cartKeys = _redisCacheService.ScanKeys(keyPattern);

             foreach (var key in cartKeys)
             {
                 var cart = await _redisCacheService.GetDataAsync<List<CartItemReturnDto>>(key);
                 if (cart == null) continue;

                 var updated = false;

                 foreach (var productId in productIds)
                 {
                     foreach (var item in cart.Where(i => i.ProductID == productId))
                     {
                         var product = await _productRepository.GetAsync(productId) ?? throw new EntityNotFoundException("Product not found");

                         var productPromotions = await _productPromotionRepository.GetPromotionByProductIdAsync(product.Id);

                         var totalDiscount = CalculateTotalDiscount(productPromotions, []);

                         var temp = CalculateProductDiscountedPrice(product.Price, totalDiscount);

                         var discountedPrice = temp == 0 ? product.Price : temp;

                         item.PName = product.PName;
                         item.Price = product.Price;
                         item.Discount = totalDiscount;
                         item.DiscountedPrice = discountedPrice;
                         item.Weight = product.Weight;
                         item.ImageURL = product.ImageURL;

                         updated = true;
                     }
                 }
                 if (updated)
                 {
                     await _redisCacheService.SetDataAsync(key, cart);
                 }
             }
         }
 */
        private async Task<CartItemReturnDto?> MapCartItemReturnDto(CartItem cartItem)
        {
            var product = await _productRepository.GetAsync(cartItem.ProductID);

            var store = await _storeRepository.GetAsync(cartItem.StoreID);

            if (product == null || store == null)
            {
                return null;
            }

            var productPromotions = await _productPromotionRepository.GetPromotionByProductIdAsync(cartItem.ProductID);

            var totalDiscount = _mapProductService.CalculateTotalDiscount(productPromotions);

            var temp = _mapProductService.CalculateProductDiscountedPrice(product.Price, totalDiscount);

            var discountedPrice = temp == 0 ? product.Price : temp;

            var productAtStore = await _atRepository.GetAtByProductIDAndStoreIDAsync(cartItem.ProductID, cartItem.StoreID);

            if (productAtStore == null)
            {
                return null;
            }

            int amount = productAtStore.NumberAtStore;

            var amountInCache = await _reserveCartCacheService.GetQuantityFromCache(cartItem.ProductID, cartItem.StoreID);
            amount -= amountInCache;

            CartItemReturnDto returnItem = new()
            {
                CartItemId = cartItem.CartItemID,
                ProductID = cartItem.ProductID,
                PName = product.PName,
                Quantity = cartItem.Quantity,
                Price = product.Price,
                StoreID = cartItem.StoreID,
                StoreName = store.Name,
                Discount = totalDiscount,
                DiscountedPrice = discountedPrice,
                Weight = product.Weight,
                ImageURL = product.ImageURL,
                HasStock = cartItem.Quantity <= amount
            };

            return returnItem;
        }

        public async Task<RevalidateReturnDto> RevalidateCartWithCustomerIdAsync(CartReturnDto cart)
        {
            var key = $"Cart:{cart.CustomerId}";
            RevalidateReturnDto revalidateReturnDto = new();

            for (int i = cart.CartItems.Count - 1; i >= 0; i--)
            {
                // Fetch the latest product details from the repository
                var item = cart.CartItems[i];
                var product = await _productRepository.GetAsync(item.ProductID);
                if (product == null)
                {
                    cart.CartItems.RemoveAt(i);
                    /*                    cart.TotalPrice -= item.DiscountedPrice * item.Quantity;*/
                    revalidateReturnDto.ProductsNotAvailable.Add(item);
                    continue;
                }

                var productPromotions = await _productPromotionRepository.GetPromotionByProductIdAsync(product.Id);

                var totalDiscount = _mapProductService.CalculateTotalDiscount(productPromotions);

                var temp = _mapProductService.CalculateProductDiscountedPrice(product.Price, totalDiscount);

                var discountedPrice = temp == 0 ? product.Price : temp;

                var productAtStore = await _atRepository.GetAtByProductIDAndStoreIDAsync(item.ProductID, item.StoreID);

                if (productAtStore == null)
                {
                    cart.CartItems.RemoveAt(i);
                    /*                    cart.TotalPrice -= item.DiscountedPrice * item.Quantity;*/
                    revalidateReturnDto.ProductsNotAvailable.Add(item);
                    continue;
                }

                bool hasStock = item.Quantity <= productAtStore.NumberAtStore;
                if (!hasStock)
                {
                    revalidateReturnDto.OutOfStockCartItem.Add(item);
                }

                // Update the product details in the cart item
                if (item.PName != product.PName)
                {
                    item.PName = product.PName;
                    revalidateReturnDto.ProductUpdated.Add(item);
                }
                if (item.Price != product.Price)
                {
                    item.Price = product.Price;
                    revalidateReturnDto.ProductUpdated.Add(item);
                }
                if (item.Discount != totalDiscount)
                {
                    item.Discount = totalDiscount;
                    item.DiscountedPrice = discountedPrice;
                    revalidateReturnDto.ProductUpdated.Add(item);
                }
                if (item.Weight != product.Weight)
                {
                    item.Weight = product.Weight;
                    revalidateReturnDto.ProductUpdated.Add(item);
                }
                if (item.ImageURL != product.ImageURL)
                {
                    item.ImageURL = product.ImageURL;
                    revalidateReturnDto.ProductUpdated.Add(item);
                }
            }
            revalidateReturnDto.Cart = cart;
            // Save the updated cart back to Redis
            await _redisCacheService.SetDataAsync(key, cart);

            return revalidateReturnDto;
        }

        public async Task<CartReturnDto?> GetCartAsync(Guid customerId)
        {
            var key = $"Cart:{customerId}";

            // Try to get the cart from Redis
            var cachedCart = await _redisCacheService.GetDataAsync<CartReturnDto>(key);
            if (cachedCart != null)
            {
                foreach (var item in cachedCart.CartItems)
                {
                    var productAtStore = await _atRepository.GetAtByProductIDAndStoreIDAsync(item.ProductID, item.StoreID);
                    if (productAtStore == null)
                    {
                        await RemoveProductFromCartAsync(customerId, item.ProductID, item.StoreID, item.DiscountedPrice, item.Quantity);
                        continue;
                    }

                    int amount = productAtStore.NumberAtStore;

                    var amountInCache = await _reserveCartCacheService.GetQuantityFromCache(item.ProductID, item.StoreID);
                    amount -= amountInCache;

                    if (item.Quantity <= amount)
                    {
                        item.HasStock = true;
                    }
                    else
                    {
                        item.HasStock = false;
                    }
                }
                return cachedCart;
            }

            // If not in Redis, get it from the database
            var dbCart = await _cartRepository.GetCartAsync(customerId);
            if (dbCart == null || dbCart.Count == 0)
            {
                return null;
            }

            var returnCart = new List<CartItemReturnDto>();
            /*            double totalPrice = 0.0;*/
            for (int i = dbCart.Count - 1; i >= 0; i--)
            {
                var item = dbCart[i];
                var returnCartItem = await MapCartItemReturnDto(item);
                if (returnCartItem == null)
                {
                    await _cartRepository.DeleteCartItemAsync(item.CartItemID);
                    continue;
                }
                returnCart.Add(returnCartItem);
                /*                totalPrice += returnCartItem.DiscountedPrice * returnCartItem.Quantity;*/
            }

            var returnCartDto = new CartReturnDto
            {
                CustomerId = customerId,
                CartItems = returnCart,
                /*                TotalPrice = totalPrice*/
            };

            // Cache the cart in Redis
            await _redisCacheService.SetDataAsync(key, returnCartDto);

            return returnCartDto;
        }

        public async Task<List<CustomerPromotionDto>> SelectCustomerPromotion(Guid customerId, CustomerPromotionDto customerPromotion)
        {
            var key = $"SelectedPromotion:{customerId}";
            var currentCustomerPromotions = await _redisCacheService.GetDataAsync<SelectedPromotionListDto>(key);
            if (currentCustomerPromotions == null)
            {
                currentCustomerPromotions = new SelectedPromotionListDto
                {
                    CustomerId = customerId,
                    CustomerPromotions = []
                };
            }
            currentCustomerPromotions.CustomerPromotions.Add(customerPromotion);
            await _redisCacheService.SetDataAsync(key, currentCustomerPromotions);
            return currentCustomerPromotions.CustomerPromotions;
        }

        public async Task<List<CustomerPromotionDto>> UnSelectCustomerPromotion(Guid customerId, List<CustomerPromotionDto> promotions)
        {
            var key = $"SelectedPromotion:{customerId}";
            var currentCustomerPromotions = await _redisCacheService.GetDataAsync<SelectedPromotionListDto>(key);
            if (currentCustomerPromotions == null)
            {
                return [];
            }
            foreach (var promotion in promotions)
            {
                currentCustomerPromotions.CustomerPromotions.RemoveAll(p => p.PromotionId == promotion.PromotionId);
            }
            await _redisCacheService.RemoveDataAsync(key);
            return currentCustomerPromotions.CustomerPromotions;
        }

        public async Task<List<CustomerPromotionDto>> GetSelectedPromotionAsync(Guid customerId)
        {
            var key = $"SelectedPromotion:{customerId}";
            var selectedPromotions = await _redisCacheService.GetDataAsync<SelectedPromotionListDto>(key);
            if (selectedPromotions == null)
            {
                return [];
            }
            return selectedPromotions.CustomerPromotions;
        }

        public async Task AddProductToCartAsync(Guid customerId, CartItemReturnDto item)
        {
            var key = $"Cart:{customerId}";

            // Try to get the cart from Redis
            var cachedCart = await _redisCacheService.GetDataAsync<CartReturnDto>(key);

            // If cart is in Redis, update it
            if (cachedCart != null)
            {
                var existingItem = cachedCart.CartItems.FirstOrDefault(p => p.ProductID == item.ProductID && p.StoreID == item.StoreID);
                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                    /*cachedCart.TotalPrice += existingItem.DiscountedPrice * item.Quantity;*/
                }
                else
                {
                    /*var newReturnItem = await MapCartItemReturnDto(item);*/
                    item.CartItemId = _guidGenerator.Create();
                    cachedCart.CartItems.Add(item);
                    /*cachedCart.TotalPrice += item.DiscountedPrice * item.Quantity;*/
                }
                await _redisCacheService.SetDataAsync(key, cachedCart);
            }

            // Add new item to cart in database
            var dbCart = await _cartRepository.GetCartAsync(customerId);
            var existingDbItem = dbCart.FirstOrDefault(p => p.ProductID == item.ProductID && p.StoreID == item.StoreID);
            if (existingDbItem != null)
            {
                existingDbItem.Quantity += item.Quantity;
                await _cartRepository.UpdateCartItemAsync(existingDbItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartItemID = _guidGenerator.Create(),
                    CustomerId = customerId,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    StoreID = item.StoreID
                };
                await _cartRepository.AddCartItemAsync(newCartItem);
            }
        }

        /*        public async Task<List<CartItemReturnDto>> UpdateProductQuantityAsync(Guid customerId, Guid productId, int quantity)
                {
                    var key = $"Cart:{customerId}";
                    var cart = await GetCartAsync(customerId);

                    var item = cart.FirstOrDefault(p => p.ProductID == productId);
                    if (item != null)
                    {
                        item.Quantity = quantity;
                    }

                    await _redisCacheService.SetDataAsync(key, cart);

                    return cart;
                }*/

        public async Task RemoveProductFromCartAsync(Guid customerId, Guid productId, Guid storeId, double discountedPrice, int quantity)
        {
            var key = $"Cart:{customerId}";
            var cart = await GetCartAsync(customerId);

            // Update the redis cache if found
            if (cart != null)
            {
                cart.CartItems.RemoveAll(p => p.ProductID == productId && p.StoreID == storeId);
                /*cart.TotalPrice -= discountedPrice * quantity;*/
                if (cart.CartItems.Count == 0)
                {
                    await _redisCacheService.RemoveDataAsync(key);
                }
                else
                {
                    await _redisCacheService.SetDataAsync(key, cart);
                }
            }

            // Not found in Redis, update the database
            var dbCart = await _cartRepository.GetCartAsync(customerId);
            var item = dbCart.FirstOrDefault(p => p.ProductID == productId && p.StoreID == storeId);
            if (item != null)
            {
                await _cartRepository.DeleteCartItemAsync(item.CartItemID);
            }
        }

        public async Task ClearCartAsync(Guid customerId)
        {
            var key = $"Cart:{customerId}";
            await _redisCacheService.RemoveDataAsync(key);

            await _cartRepository.DeleteCartAsync(customerId);

            var promotionKey = $"SelectedPromotion:{customerId}";
            await _redisCacheService.RemoveDataAsync(promotionKey);
        }
    }
}
