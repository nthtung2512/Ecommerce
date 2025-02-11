﻿using FluentValidation;
using MealMate.BLL.Dtos.Product;
using MealMate.BLL.IServices;
using MealMate.BLL.IServices.Utility;
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
        private readonly IMapProductService _mapProductService;
        private readonly IValidator<Product> _productValidator;
        private readonly GuidGenerator _guidGenerator;

        public ProductAppService(IProductRepository productRepository, GuidGenerator guidGenerator, IValidator<Product> productValidator, ITransactionRepository transactionRepository, IMapProductService mapProductService)
        {
            _productRepository = productRepository;
            _guidGenerator = guidGenerator;
            _productValidator = productValidator;
            _transactionRepository = transactionRepository;
            _mapProductService = mapProductService;
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
                /* var discount = item.Discount;
                 var discountedPrice = CalculateProductDiscountedPrice(item.Product.Price, discount) == 0 ? item.Product.Price : CalculateProductDiscountedPrice(item.Product.Price, discount);*/
                var productDto = new ProductDto
                {
                    ProductID = item.Product.Id,
                    Category = item.Product.Category,
                    Description = item.Product.Description,
                    PName = item.Product.PName,
                    Price = item.Product.Price,
                    DiscountedPrice = (double)(item.SubTotal / item.NumberOfProductInBill),
                    Weight = item.Product.Weight,
                    ImageURL = item.Product.ImageURL
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
                var productDto = await _mapProductService.MapProductDto(product);
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
                var productDto = await _mapProductService.MapProductDto(product);
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
                var productDto = await _mapProductService.MapProductDto(product);
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
                var productDto = await _mapProductService.MapProductDto(product);
                productDtos.Add(productDto);
            }
            return productDtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetAsync(productId) ?? throw new EntityNotFoundException("Product not found");

            return await _mapProductService.MapProductDto(product);
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

        public async Task<List<ProductDto>> GetProductsByListNameAsync(List<string> productNames)
        {
            var products = await _productRepository.GetProductsByListNameAsync(productNames);
            if (products.Count == 0)
            {
                throw new EntityNotFoundException("No product found for this list of product names");
            }
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                var productDto = await _mapProductService.MapProductDto(product);
                productDtos.Add(productDto);
            }

            return productDtos;
        }

        public async Task<ProductDto> CreateProductAsync(ProductCreationDto createData)
        {
            var newProduct = new Product(_guidGenerator.Create()) { Category = createData.Category, Description = createData.Description, PName = createData.PName, Price = createData.Price, Weight = createData.Weight, ImageURL = createData.ImageURL, IsDeleted = false };

            var validationResult = await _productValidator.ValidateAsync(newProduct);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when creating new product: {string.Join(", ", validationResult.Errors)}"
                );
            }
            var newProductDto = await _mapProductService.MapProductDto(newProduct);
            await _productRepository.CreateAsync(newProduct);
            return newProductDto;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id) ?? throw new EntityNotFoundException("Product not found");
            await _productRepository.DeleteAsync(product);
        }

        public async Task<ProductDto> UpdateProductAsync(Guid id, ProductUpdateDto updateData)
        {
            var product = await _productRepository.GetAsync(id) ?? throw new EntityNotFoundException("Product not found");

            product.Category = updateData.Category ?? product.Category;
            product.Description = updateData.Description ?? product.Description;
            product.Price = updateData.Price ?? product.Price;
            product.Weight = updateData.Weight ?? product.Weight;
            product.ImageURL = updateData.ImageURL ?? product.ImageURL;

            var validationResult = await _productValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                throw new EntityValidationException(
                    $"Validation exception when updating product: {string.Join(", ", validationResult.Errors)}"
                );
            }
            var productDto = await _mapProductService.MapProductDto(product);
            await _productRepository.UpdateAsync(product);

            /*            await _cartService.RevalidateCartsWithProductIdsAsync([id]);*/
            return productDto;
        }

        public async Task DeleteProductAtStoreAsync(Guid productId, Guid storeId)
        {
            await _productRepository.DeleteProductAtStoreAsync(productId, storeId);
        }
    }
}
