using MealMate.BLL.Dtos.Product;
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

        // TempTop5Product
        Task<List<TempTop5Product>> GetTempTop5ProductsAsync(int year);
    }
}
