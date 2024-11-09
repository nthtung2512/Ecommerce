using MealMate.DAL.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MealMate.BLL.Services.ScheduleJob
{
    public class PromotionCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PromotionCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Calculate time until the next midnight
                var now = DateTime.UtcNow.AddHours(7);
                var midnight = now.Date.AddDays(1);
                var timeUntilMidnight = midnight - now;

                // Wait until midnight
                await Task.Delay(timeUntilMidnight, stoppingToken);

                // Perform the cleanup
                await DeleteExpiredPromotionsAsync();

                // Repeat every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task DeleteExpiredPromotionsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var promotionRepository = scope.ServiceProvider.GetRequiredService<IProductPromotionRepository>();
                var expiredPromotions = await promotionRepository.GetExpiredPromotionsAsync();

                foreach (var promotion in expiredPromotions)
                {
                    await promotionRepository.DeleteAsync(promotion);
                }
            }
        }
    }
}
