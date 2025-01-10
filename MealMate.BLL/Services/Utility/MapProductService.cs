using MealMate.BLL.Dtos.Product;
using MealMate.BLL.IServices.Utility;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.IRepositories;

namespace MealMate.BLL.Services.Utility
{
    internal class MapProductService : IMapProductService
    {
        private readonly IProductPromotionRepository _productPromotionRepository;

        public MapProductService(IProductPromotionRepository productPromotionRepository)
        {
            _productPromotionRepository = productPromotionRepository;
        }

        public double CalculateProductDiscountedPrice(double price, decimal discount)
        {
            return price * (1 - (double)discount);
        }

        public decimal CalculateTotalDiscount(List<ProductPromotion> productPromotions)
        {
            var totalDiscount = 0.00m;
            totalDiscount += productPromotions.Sum(promotion => promotion.Discount);

            // Ensure the total discount does not exceed 0.99
            return Math.Min(totalDiscount, 0.99m);
        }

        public async Task<ProductDto> MapProductDto(Product product)
        {
            var productPromotions = await _productPromotionRepository.GetPromotionByProductIdAsync(product.Id);

            var totalDiscount = CalculateTotalDiscount(productPromotions);

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

        public double ShortCutCalculateProductDiscountedPrice(List<ProductPromotion> productPromotions, double price)
        {
            var discount = CalculateTotalDiscount(productPromotions);
            return CalculateProductDiscountedPrice(price, discount);
        }
    }
}
