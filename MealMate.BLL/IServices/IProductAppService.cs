using MealMate.BLL.Dtos.Product;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Transactions;

namespace MealMate.BLL.IServices
{
    public interface IProductAppService
    {
        // Product
        Task<ProductDto> GetProductByIdAsync(Guid productId);
        Task<List<ProductDto>> GetListProductByCategoryAsync(string category);
        Task<List<ProductDto>> GetListProductByPromotionIDAsync(Guid id);
        Task<List<ProductDto>> GetListProductHavePromotionAsync();
        Task<List<ProductDto>> GetListProductByStoreIDAsync(Guid storeId);
        Task<List<ProductDto>> GetAllItemsByBillIdAsync(Guid transactionId);
        Task<List<ProductDto>> GetProductsByListNameAsync(List<string> productNames);
        Task<ProductDto> CreateProductAsync(ProductCreationDto createData);
        Task<ProductDto> UpdateProductAsync(Guid id, ProductUpdateDto updateData);
        Task DeleteProductAsync(Guid id);
        Task DeleteProductAtStoreAsync(Guid productId, Guid storeId);

        // Calculate Product price
        Task<ProductDto> MapProductDto(Product product);
        decimal CalculateTotalDiscount(List<ProductPromotion> productPromotions, List<ProductCategoryPromotion> productCategoryPromotions);
        double CalculateProductDiscountedPrice(double price, decimal discount);
        double ShortCutCalculateProductDiscountedPrice(List<ProductPromotion> productPromotions, List<ProductCategoryPromotion> productCategoryPromotions, double price);

        // TempTop5Product
        Task<List<TempTop5Product>> GetTempTop5ProductsAsync(int year);
    }
}
