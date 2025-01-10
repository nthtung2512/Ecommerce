using MealMate.BLL.Dtos.Product;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Transactions;

namespace MealMate.BLL.IServices.Utility
{
    public interface IMapProductService
    {
        double ShortCutCalculateProductDiscountedPrice(List<ProductPromotion> productPromotions, double price);

        decimal CalculateTotalDiscount(List<ProductPromotion> productPromotion);

        double CalculateProductDiscountedPrice(double price, decimal discount);

        Task<ProductDto> MapProductDto(Product product);
    }
}
