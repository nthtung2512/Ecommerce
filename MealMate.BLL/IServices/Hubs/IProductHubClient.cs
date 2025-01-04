using MealMate.Base.Hub;

namespace MealMate.BLL.IServices.Hubs
{
    public interface IProductHubClient : IHubClient
    {
        Task ReceiveChangeStock(Guid productId, int newStock);
    }
}
