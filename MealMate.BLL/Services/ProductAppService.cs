using AutoMapper;
using FluentValidation;
using MealMate.BLL.Dtos.Product;
using MealMate.BLL.IServices;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Transactions;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils.Exceptions;
using MealMate.DAL.Utils.GuidUtil;

namespace MealMate.BLL.Services
{
    internal class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IProductPromotionRepository _productPromotionRepository;
        private readonly ICategoryPromotionRepository _categoryPromotionRepository;
        private readonly IValidator<Product> _productValidator;
        private readonly GuidGenerator _guidGenerator;
        private readonly Mapper _mapper;

        public ProductAppService(IProductRepository productRepository, GuidGenerator guidGenerator, Mapper mapper, IValidator<Product> productValidator, ITransactionRepository transactionRepository, IProductPromotionRepository productPromotionRepository, ICategoryPromotionRepository categoryPromotionRepository)
        {
            _productRepository = productRepository;
            _guidGenerator = guidGenerator;
            _mapper = mapper;
            _productValidator = productValidator;
            _transactionRepository = transactionRepository;
            _productPromotionRepository = productPromotionRepository;
            _categoryPromotionRepository = categoryPromotionRepository;
        }

        public async Task<ProductDto> MapProductDto(Product product)
        {
            var productPromotions = await _productPromotionRepository.GetPromotionByProductIdAsync(product.Id);
            var productCategoryPromotions = await _categoryPromotionRepository.GetCategoryPromotionByCategoryAsync(product.Category);

            var totalDiscount = CalculateTotalDiscount(productPromotions, productCategoryPromotions);

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
                Weight = product.Weight
            };
            return productDto;
        }

        public async Task<List<ProductDto>> GetAllItemsByBillIdAsync(Guid transactionId)
        {
            var includes = await _transactionRepository.GetAllItemsByBillIdAsync(transactionId);
            if (includes.Count == 0)
            {
                throw new EntityNotFoundException("No product found for this bill");
            }
            var productDtos = new List<ProductDto>();
            foreach (var item in includes)
            {
                var discount = item.Discount;
                var discountedPrice = CalculateProductDiscountedPrice(item.Product.Price, discount) == 0 ? item.Product.Price : CalculateProductDiscountedPrice(item.Product.Price, discount);
                var productDto = new ProductDto
                {
                    ProductID = item.Product.Id,
                    Category = item.Product.Category,
                    Description = item.Product.Description,
                    PName = item.Product.PName,
                    Price = item.Product.Price,
                    Discount = discount,
                    DiscountedPrice = Math.Round(discountedPrice, 2),
                    Weight = item.Product.Weight
                };
                productDtos.Add(productDto);
            }
            return productDtos;
        }

        public async Task<List<ProductDto>> GetListProductByCategoryAsync(string category)
        {
            var products = await _productRepository.GetListProductByCategoryAsync(category);
            if (products.Count == 0)
            {
                throw new EntityNotFoundException("No product found for this category");
            }
            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = await MapProductDto(product);
                productDtos.Add(productDto);
            }
            return productDtos;
        }

        public async Task<List<ProductDto>> GetListProductByPromotionIDAsync(Guid id)
        {
            var products = await _transactionRepository.GetListProductByPromotionIDAsync(id);
            if (products.Count == 0)
            {
                throw new EntityNotFoundException("No product found for this promotion");
            }
            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = await MapProductDto(product);
                productDtos.Add(productDto);
            }
            return productDtos;
        }

        public async Task<List<ProductDto>> GetListProductByStoreIDAsync(Guid storeId)
        {
            var products = await _productRepository.GetListProductByStoreIDAsync(storeId);
            if (products.Count == 0)
            {
                throw new EntityNotFoundException("No product found for this store");
            }
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                var productDto = await MapProductDto(product);
                productDtos.Add(productDto);
            }
            return productDtos;
        }

        public async Task<List<ProductDto>> GetListProductHavePromotionAsync()
        {
            var products = await _productRepository.GetListProductHavePromotionAsync();
            if (products.Count == 0)
            {
                throw new EntityNotFoundException("No product has promotion");
            }
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                var productDto = await MapProductDto(product);
                productDtos.Add(productDto);
            }
            return productDtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetAsync(productId) ?? throw new EntityNotFoundException("Product not found");

            return await MapProductDto(product);
        }

        public async Task<List<TempTop5Product>> GetTempTop5ProductsAsync(int year)
        {
            var tempTop5Products = await _productRepository.GetTempTop5ProductsAsync(year);
            if (tempTop5Products.Count == 0)
            {
                throw new EntityNotFoundException("No product found for this year");
            }
            return tempTop5Products;
        }

        public async Task<ProductDto> CreateProductAsync(ProductCreationDto createData)
        {
            var newProduct = new Product(_guidGenerator.Create()) { Category = createData.Category, Description = createData.Description, PName = createData.PName, Price = createData.Price, Weight = createData.Weight, IsDeleted = false };

            var validationResult = await _productValidator.ValidateAsync(newProduct);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when creating new product: {string.Join(", ", validationResult.Errors)}"
                );
            }
            await _productRepository.CreateAsync(newProduct);
            return await MapProductDto(newProduct);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id) ?? throw new EntityNotFoundException("Product not found");
            await _productRepository.DeleteAsync(product);
        }

        public async Task<ProductDto> UpdateProductAsync(Guid id, ProductUpdateDto updateData)
        {
            var product = await _productRepository.GetAsync(id) ?? throw new EntityNotFoundException("Product not found");

            var validationResult = await _productValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when updating product: {string.Join(", ", validationResult.Errors)}"
                );
            }

            await _productRepository.UpdateAsync(product);
            return await MapProductDto(product);
        }

        public double ShortCutCalculateProductDiscountedPrice(List<ProductPromotion> productPromotions, List<ProductCategoryPromotion> productCategoryPromotions, double price)
        {
            var discount = CalculateTotalDiscount(productPromotions, productCategoryPromotions);
            return CalculateProductDiscountedPrice(price, discount);
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
            return price * (double)discount;
        }

    }
}
