using AutoMapper;
using MealMate.BLL.Dtos.Promotion;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services
{
    internal class CategoryPromotionAppService : ICategoryPromotionAppService
    {
        private readonly ICategoryPromotionRepository _categoryPromotionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly GuidGenerator _guidGenerator;

        public CategoryPromotionAppService(ICategoryPromotionRepository categoryPromotionRepository, IMapper mapper, GuidGenerator guidGenerator, IProductRepository productRepository)
        {
            _categoryPromotionRepository = categoryPromotionRepository;
            _mapper = mapper;
            _guidGenerator = guidGenerator;
            _productRepository = productRepository;
        }

        public async Task<ProductPromotionCreationDto> CreateProductPromotionByCategoryAsync(CategoryPromotionCreationDto promotionData, string category)
        {
            var listProducts = await _productRepository.GetListProductByCategoryAsync(category);
            if (listProducts.Count == 0)
            {
                throw new EntryPointNotFoundException("No product found for this category");
            }

            var promotionId = _guidGenerator.Create();
            var newPromotion = new ProductCategoryPromotion(promotionId)
            {
                Description = promotionData.Description,
                Discount = promotionData.Discount,
                Name = promotionData.Name,
                StartDay = promotionData.StartDay,
                EndDay = promotionData.EndDay,
                Category = category
            };

            foreach (var product in listProducts)
            {
                newPromotion.PromoteProductCategories.Add(new PromoteCategory
                {
                    ProductId = product.Id,
                    PromotionId = promotionId,
                    Product = product,
                    ProductCategoryPromotion = newPromotion
                });
            }

            await _categoryPromotionRepository.CreateAsync(newPromotion);

            return _mapper.Map<ProductPromotionCreationDto>(newPromotion);
        }

        public async Task DeletePromotionAsync(Guid id)
        {
            var promotion = await _categoryPromotionRepository.GetCategoryPromotionByIdAsync(id) ?? throw new EntryPointNotFoundException("No promotion found");
            await _categoryPromotionRepository.DeleteAsync(promotion);
        }

        public async Task<List<CategoryPromotionCreationDto>> GetAllCategoryPromotionsAsync()
        {
            var promotions = await _categoryPromotionRepository.GetAllCategoryPromotionsAsync();
            if (promotions.Count == 0)
            {
                throw new EntryPointNotFoundException("No promotion found");
            }
            return _mapper.Map<List<CategoryPromotionCreationDto>>(promotions);
        }

        public async Task<CategoryPromotionCreationDto> GetCategoryPromotionByIdAsync(Guid id)
        {
            var promotion = await _categoryPromotionRepository.GetCategoryPromotionByIdAsync(id);
            if (promotion == null)
            {
                throw new EntryPointNotFoundException("No promotion found");
            }
            return _mapper.Map<CategoryPromotionCreationDto>(promotion);
        }

        public async Task<List<CategoryPromotionCreationDto>> GetPromotionsByProductCategory(string category)
        {
            var promotions = await _categoryPromotionRepository.GetCategoryPromotionByCategoryAsync(category);
            if (promotions.Count == 0)
            {
                throw new EntryPointNotFoundException("No promotion found");
            }
            return _mapper.Map<List<CategoryPromotionCreationDto>>(promotions);
        }
    }
}
