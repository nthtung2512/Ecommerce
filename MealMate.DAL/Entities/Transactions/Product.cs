using FluentValidation;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Stores;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils;
using MealMate.DAL.Utils.EFCore;

namespace MealMate.DAL.Entities.Transactions
{
    public class Product(Guid id) : Entity<Guid>(id), IDeletableEntity
    {
        public required string Image { get; set; }
        public required string Consistency { get; set; }
        public required string Name { get; set; }
        public required string NameClean { get; set; }
        public required string OriginalName { get; set; }
        public required int Amount { get; set; }
        public string Unit { get; set; } = string.Empty;
        public required double Price { get; set; }
        public required string Aisle { get; set; }
        public string Description { get; set; } = "Fresh food from MealMate";
        public bool IsDeleted { get; set; }
        public ICollection<PromoteProduct> PromoteProducts { get; } = [];
        public ICollection<PromoteCategory> PromoteCategories { get; } = [];
        public ICollection<AT> ATs { get; } = [];
        public ICollection<Include> Includes { get; } = [];
    }

    internal class ProductValidator : AbstractValidator<Product>
    {
        private readonly IProductRepository _productRepository;
        public ProductValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(product => product.Price)
                .GreaterThan(0.0)
                .WithMessage("Price must be greater than 0");

            RuleFor(product => product.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0");

            RuleFor(product => product)
                .MustAsync((product, token) => IsProductNameUnique(product.Name, product.Id, token))
                .WithMessage("Product name must be unique");

        }
        private async Task<bool> IsProductNameUnique(string productName, Guid productId, CancellationToken token)
        {
            var existingProduct = await _productRepository.GetProductByNameAsync(productName);
            // If no product with the name exists or the existing product is the current product, return true.
            return existingProduct == null || existingProduct.Id == productId;
        }
    }
}
