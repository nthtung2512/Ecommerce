using MealMate.BLL.IServices.Hubs;
using MealMate.DAL.Entities.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MealMate.BLL.Services.Hubs
{
    public class ProductHub : Hub<IProductHubClient>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            var user = Context.User;

            if (user == null || !user.Identity.IsAuthenticated)
            {
                Context.Abort();
                return Task.CompletedTask;
            }

            // Retrieve the user ID from claims
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                Context.Abort();
                return Task.CompletedTask;
            }

            // Check if the user has the "Customer" role
            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser == null || !(await _userManager.IsInRoleAsync(applicationUser, "Customer")))
            {
                Context.Abort(); // Disconnect non-customers
                return Task.CompletedTask;
            }

            // Add the customer to the "Customers" group
            await Groups.AddToGroupAsync(Context.ConnectionId, "Customers");
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            // Remove the customer from the "Customers" group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Customers");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task ChangeStock(Guid productId, int newStock)
        {
            // Broadcast to the "Customers" group only
            await Clients.Group("Customers").ReceiveChangeStock(productId, newStock);
        }
    }
}
