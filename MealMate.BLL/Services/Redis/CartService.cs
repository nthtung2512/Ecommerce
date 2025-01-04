using MealMate.BLL.Dtos.Bills;
using MealMate.BLL.Dtos.Product;
using MealMate.BLL.IServices.Redis;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services.Redis
{
    internal class CartService : ICartService
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IProductPromotionRepository _productPromotionRepository;
        private readonly GuidGenerator _guidGenerator;

        public CartService(IRedisCacheService redisCacheService, IProductRepository productRepository, GuidGenerator guidGenerator, IStoreRepository storeRepository, IProductPromotionRepository productPromotionRepository)
        {
            _redisCacheService = redisCacheService;
            _productRepository = productRepository;
            _guidGenerator = guidGenerator;
            _storeRepository = storeRepository;
            _productPromotionRepository = productPromotionRepository;
        }

        public decimal CalculateTotalDiscount(List<ProductPromotion> productPromotions, List<ProductCategoryPromotion> productCategoryPromotions)
        {
            var totalDiscount = 0.00m;
            totalDiscount += productPromotions.Sum(promotion => promotion.Discount);
            totalDiscount += productCategoryPromotions.Sum(promotion => promotion.Discount);

            // Ensure the total discount does not exceed 0.99
            return Math.Min(totalDiscount, 0.99m);
        }

        public double CalculateProductDiscountedPrice(double price, decimal discount)
        {
            return price * (1 - (double)discount);
        }

        public async Task<ProductDto> MapProductDto(Product product)
        {
            var productPromotions = await _productPromotionRepository.GetPromotionByProductIdAsync(product.Id);

            var totalDiscount = CalculateTotalDiscount(productPromotions, []);

            var discountedPrice = CalculateProductDiscountedPrice(product.Price, totalDiscount) == 0 ? product.Price : CalculateProductDiscountedPrice(product.Price, totalDiscount);

            var productDto = new ProductDto
            {
                ProductID = product.Id,
                Category = product.Category,
                Description = product.Description,
                PName = product.PName,
                Price = product.Price,
                Discount = totalDiscount,
                DiscountedPrice = Math.Round(discountedPrice, 2),
                Weight = product.Weight,
                ImageURL = product.ImageURL
            };
            return productDto;
        }

        public async Task RevalidateCartsWithProductIdsAsync(List<Guid> productIds)
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


        public async Task<List<CartItemReturnDto>> RevalidateCartWithCustomerIdAsync(Guid customerId)
        {
            var key = $"Cart:{customerId}";
            var cart = await GetCartAsync(customerId);

            if (cart.Count == 0)
            {
                return [];
            }

            foreach (var item in cart)
            {
                // Fetch the latest product details from the repository
                var product = await _productRepository.GetAsync(item.ProductID);
                if (product == null)
                {
                    // If the product no longer exists, optionally remove it from the cart
                    continue;
                }

                var productPromotions = await _productPromotionRepository.GetPromotionByProductIdAsync(product.Id);

                var totalDiscount = CalculateTotalDiscount(productPromotions, []);

                var temp = CalculateProductDiscountedPrice(product.Price, totalDiscount);

                var discountedPrice = temp == 0 ? product.Price : temp;

                // Update the product details in the cart item
                item.PName = product.PName;
                item.Price = product.Price;
                item.Discount = totalDiscount;
                item.DiscountedPrice = discountedPrice;
            }

            // Save the updated cart back to Redis
            await _redisCacheService.SetDataAsync(key, cart);

            return cart;
        }

        public async Task<List<CartItemReturnDto>> GetCartAsync(Guid customerId)
        {
            var key = $"Cart:{customerId}";
            var cart = await _redisCacheService.GetDataAsync<List<CartItemReturnDto>>(key);
            return cart ?? new List<CartItemReturnDto>();
        }

        public async Task<List<CartItemReturnDto>> GetCartWithRevalidationAsync(Guid customerId)
        {
            return await RevalidateCartWithCustomerIdAsync(customerId);
        }

        public async Task<List<CartItemReturnDto>> AddProductToCartAsync(Guid customerId, CartItem item)
        {
            var key = $"Cart:{customerId}";
            var cart = await GetCartAsync(customerId);

            var existingItem = cart.FirstOrDefault(p => p.ProductID == item.ProductID);
            if (existingItem == null)
            {
                var product = await _productRepository.GetAsync(item.ProductID) ?? throw new EntityNotFoundException("No product found");

                var fullProductDto = await MapProductDto(product);

                var store = await _storeRepository.GetAsync(item.StoreID) ?? throw new EntityNotFoundException("No store found");

                var newItem = new CartItemReturnDto
                {
                    CartId = _guidGenerator.Create(),
                    ProductID = item.ProductID,
                    PName = fullProductDto.PName,
                    Quantity = item.Quantity,
                    Price = fullProductDto.Price,
                    StoreID = item.StoreID,
                    StoreName = store.Name,
                    Discount = fullProductDto.Discount,
                    DiscountedPrice = fullProductDto.DiscountedPrice,
                    Weight = fullProductDto.Weight,
                    ImageURL = fullProductDto.ImageURL,
                };

                cart.Add(newItem);
            }
            else
            {
                existingItem.Quantity += item.Quantity;
            }

            await _redisCacheService.SetDataAsync(key, cart);
            return cart;
        }

        public async Task<List<CartItemReturnDto>> UpdateProductQuantityAsync(Guid customerId, Guid productId, int quantity)
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
        }

        public async Task<List<CartItemReturnDto>> RemoveProductFromCartAsync(Guid customerId, Guid productId)
        {
            var key = $"Cart:{customerId}";
            var cart = await GetCartAsync(customerId);

            cart.RemoveAll(p => p.ProductID == productId);
            await _redisCacheService.SetDataAsync(key, cart);

            return cart;
        }

        public async Task<List<CartItemReturnDto>> ClearCartAsync(Guid customerId)
        {
            var key = $"Cart:{customerId}";
            await _redisCacheService.RemoveDataAsync(key);
            return [];
        }
    }

}
