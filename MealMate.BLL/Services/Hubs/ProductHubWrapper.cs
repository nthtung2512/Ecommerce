using MealMate.Base.Hub;
using MealMate.BLL.IServices.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MealMate.BLL.Services.Hubs
{
    public class ProductHubWrapper(IHubContext<ProductHub, IProductHubClient> hubContext) : IHubContextWrapper<IProductHubClient>
    {
        private readonly IHubContext<ProductHub, IProductHubClient> _hubContext = hubContext;

        public IProductHubClient Client(string connId)
        {
            return _hubContext.Clients.Client(connId);
        }

        public IProductHubClient Group(string groupName)
        {
            return _hubContext.Clients.Group(groupName);
        }
    }
}
