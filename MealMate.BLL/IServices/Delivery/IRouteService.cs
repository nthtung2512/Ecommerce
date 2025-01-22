using MealMate.BLL.Dtos.Delivery;

namespace MealMate.BLL.IServices.Delivery
{
    public interface IRouteService
    {
        Task<RouteResult> GetOptimalRouteAsync(string shopAddress, List<string> deliveryAddresses);
    }
}
