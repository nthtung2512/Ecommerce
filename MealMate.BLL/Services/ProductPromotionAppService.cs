using AutoMapper;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services
{
    internal class ProductPromotionAppService : IProductPromotionAppService
    {
        private readonly IProductPromotionRepository _productPromotionRepository;
        private readonly IProductRepository _productRepository;
        private readonly GuidGenerator _guidGenerator;
        private readonly IMapper _mapper;
        public ProductPromotionAppService(IProductPromotionRepository productPromotionRepository, IMapper mapper, IProductRepository productRepository, GuidGenerator guidGenerator)
        {
            _productPromotionRepository = productPromotionRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<ProductPromotionCreationDto> CreateProductPromotionByProductIdAsync(ProductPromotionCreationDto promotionData, Guid productId)
        {
            var product = await _productRepository.GetAsync(productId) ?? throw new EntryPointNotFoundException("No product found");
            var promotionId = _guidGenerator.Create();
            var newPromotion = new ProductPromotion(promotionId)
            {
                Description = promotionData.Description,
                Discount = promotionData.Discount,
                Name = promotionData.Name,
                StartDay = promotionData.StartDay,
                EndDay = promotionData.EndDay,
            };

            newPromotion.PromoteProducts.Add(new PromoteProduct
            {
                ProductId = productId,
                Product = product,
                PromotionId = promotionId,
                ProductPromotion = newPromotion
            });

            await _productPromotionRepository.CreateAsync(newPromotion);

            return _mapper.Map<ProductPromotionCreationDto>(newPromotion);
        }

        public async Task DeletePromotionAsync(Guid id)
        {
            var promotion = await _productPromotionRepository.GetProductPromotionByIdAsync(id) ?? throw new EntryPointNotFoundException("No promotion found");
            await _productPromotionRepository.DeleteAsync(promotion);
        }

        public async Task<List<ProductPromotionCreationDto>> GetAllProductPromotionsAsync()
        {
            var promotions = await _productPromotionRepository.GetAllProductPromotionsAsync();
            if (promotions.Count == 0)
            {
                throw new EntryPointNotFoundException("No promotion found");
            }

            return _mapper.Map<List<ProductPromotionCreationDto>>(promotions);
        }

        public async Task<ProductPromotionCreationDto> GetProductPromotionByIdAsync(Guid id)
        {
            var promotion = await _productPromotionRepository.GetProductPromotionByIdAsync(id) ?? throw new EntryPointNotFoundException("No promotion found");
            return _mapper.Map<ProductPromotionCreationDto>(promotion);
        }

        public async Task<List<ProductPromotionCreationDto>> GetPromotionsByProductId(Guid productId)
        {
            var promotions = await _productPromotionRepository.GetPromotionByProductIdAsync(productId);
            if (promotions.Count == 0)
            {
                throw new EntryPointNotFoundException("No promotion found");
            }
            return _mapper.Map<List<ProductPromotionCreationDto>>(promotions);
        }
    }
}
