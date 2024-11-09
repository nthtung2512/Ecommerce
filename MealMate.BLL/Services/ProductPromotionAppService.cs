using MealMate.BLL.Dtos.Product;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services
{
    internal class ProductPromotionAppService : IProductPromotionAppService
    {
        private readonly IProductPromotionRepository _productPromotionRepository;
        private readonly IProductAppService _productAppService;
        private readonly IProductRepository _productRepository;
        private readonly GuidGenerator _guidGenerator;
        public ProductPromotionAppService(IProductPromotionRepository productPromotionRepository, IProductRepository productRepository, GuidGenerator guidGenerator, IProductAppService productAppService)
        {
            _productPromotionRepository = productPromotionRepository;
            _productRepository = productRepository;
            _guidGenerator = guidGenerator;
            _productAppService = productAppService;
        }

        public async Task<ProductPromotionDto> CreateProductPromotionByProductIdAsync(ProductPromotionCreationDto promotionData)
        {
            var promotionId = _guidGenerator.Create();

            var newPromotion = new ProductPromotion(promotionId)
            {
                Description = promotionData.Description,
                Discount = promotionData.Discount,
                Name = promotionData.Name,
                StartDay = promotionData.StartDay,
                EndDay = promotionData.EndDay
            };

            var productDtos = new List<ProductDto>();
            foreach (var productId in promotionData.ProductIdList)
            {
                var product = await _productRepository.GetAsync(productId) ?? throw new EntityNotFoundException("No product found");
                newPromotion.PromoteProducts.Add(new PromoteProduct
                {
                    ProductId = productId,
                    Product = product,
                    PromotionId = promotionId,
                    ProductPromotion = newPromotion
                });
                var productDto = await _productAppService.MapProductDto(product);
                productDto.Discount = Math.Min(productDto.Discount + promotionData.Discount, 0.99m);
                productDto.DiscountedPrice = productDto.Price - productDto.Price * (double)productDto.Discount;
                productDtos.Add(productDto);

            }

            var result = new ProductPromotionDto
            {
                PromotionID = promotionId,
                Description = newPromotion.Description,
                Discount = newPromotion.Discount,
                Name = newPromotion.Name,
                StartDay = newPromotion.StartDay,
                EndDay = newPromotion.EndDay,
                Products = productDtos
            };


            await _productPromotionRepository.CreateAsync(newPromotion);

            return result;
        }

        public async Task DeleteExpiredPromotionsAsync()
        {
            var promotions = await _productPromotionRepository.GetExpiredPromotionsAsync();
            foreach (var promotion in promotions)
            {
                await _productPromotionRepository.DeleteAsync(promotion);
            }
        }

        public async Task<List<ProductPromotionDto>> GetAllProductPromotionsAsync()
        {
            var promotionsWithProductId = await _productPromotionRepository.GetAllProductPromotionsAsync();
            if (promotionsWithProductId.Count == 0)
            {
                throw new EntityNotFoundException("No promotion found");
            }

            var productPromotionDtos = new List<ProductPromotionDto>();

            foreach (var promotion in promotionsWithProductId)
            {
                var productDtos = new List<ProductDto>();
                foreach (var promoteProduct in promotion.PromoteProducts)
                {
                    productDtos.Add(await _productAppService.MapProductDto(promoteProduct.Product));
                }
                productPromotionDtos.Add(new ProductPromotionDto
                {
                    PromotionID = promotion.Id,
                    Description = promotion.Description,
                    Discount = promotion.Discount,
                    Name = promotion.Name,
                    StartDay = promotion.StartDay,
                    EndDay = promotion.EndDay,
                    Products = productDtos
                });
            }
            return productPromotionDtos;
        }

        /* public async Task<ProductPromotionCreationDto> GetProductPromotionByIdAsync(Guid id)
         {
             var promotionsWithProductId = await _productPromotionRepository.GetProductPromotionByIdAsync(id) ?? throw new EntityNotFoundException("No promotion found");
             return _mapper.Map<ProductPromotionCreationDto>(promotionsWithProductId);
         }*/

        public async Task<List<ProductPromotionDto>> GetPromotionsByProductId(Guid productId)
        {
            var promoteProducts = await _productPromotionRepository.GetPromotionByProductIdAsync(productId);
            if (promoteProducts.Count == 0)
            {
                throw new EntityNotFoundException("No promotion found");
            }

            var productPromotionDtos = new List<ProductPromotionDto>();

            foreach (var promotion in promoteProducts)
            {
                productPromotionDtos.Add(new ProductPromotionDto
                {
                    PromotionID = promotion.Id,
                    Description = promotion.Description,
                    Discount = promotion.Discount,
                    Name = promotion.Name,
                    StartDay = promotion.StartDay,
                    EndDay = promotion.EndDay,
                });
            }

            return productPromotionDtos;

        }
    }
}
