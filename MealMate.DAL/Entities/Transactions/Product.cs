using FluentValidation;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Utils;
using MealMate.DAL.Utils.EFCore;

namespace MealMate.DAL.Entities.Transactions
{
    public class Product(Guid id) : Entity<Guid>(id), IDeletableEntity
    {
        public required string Category { get; init; }

        public string Description { get; set; } = string.Empty;

        public required string PName { get; init; }

        public required double Price { get; set; }

        public required double Weight { get; init; }
        public required string ImageURL { get; set; }
        public bool IsDeleted { get; set; }
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

            RuleFor(product => product.Weight)
                .GreaterThan(0.0)
                .WithMessage("Weight must be greater than 0");

            RuleFor(product => product.PName)
                .MustAsync(IsProductNameUnique)
                .WithMessage("Product name must be unique");
        }
        private async Task<bool> IsProductNameUnique(string productName, CancellationToken token)
        {
            return await _productRepository.GetProductByNameAsync(productName) == null;
        }
    }
}
