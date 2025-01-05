using MealMate.BLL.IServices.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MealMate.BLL.Services.Hubs
{
    public class ProductHub : Hub<IProductHubClient>
    {
        // No authentication or user manager dependency required

        // Override OnConnectedAsync if you want to log or handle connection logic
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        // Override OnDisconnectedAsync to log or handle disconnection logic
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }

        // Method for clients to join specific product-store groups
        public async Task JoinProductStoreGroup(Guid productId, Guid storeId)
        {
            var groupName = $"{productId}_{storeId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine($"Client {Context.ConnectionId} joined group {groupName}");
        }

        // Method for clients to leave specific product-store groups
        public async Task LeaveProductStoreGroup(Guid productId, Guid storeId)
        {
            var groupName = $"{productId}_{storeId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine($"Client {Context.ConnectionId} left group {groupName}");
        }

        // Method to notify specific groups about stock changes
        public async Task ChangeStock(Guid productId, Guid storeId, int newStock)
        {
            var groupName = $"{productId}_{storeId}";
            await Clients.Group(groupName).ReceiveChangeStock(productId, newStock);
            Console.WriteLine($"Stock updated for group {groupName}: {newStock}");
        }
    }

}
