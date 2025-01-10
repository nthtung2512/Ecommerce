using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace MealMate.BLL.IServices.Redis
{
    public class RedisExpirationSubscriber : BackgroundService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IServiceProvider _serviceProvider;

        public RedisExpirationSubscriber(
            IConnectionMultiplexer connectionMultiplexer,
            IServiceProvider serviceProvider)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _connectionMultiplexer.GetSubscriber();

            // Subscribe to expiration events
            await subscriber.SubscribeAsync(new RedisChannel("__keyevent@0__:expired", RedisChannel.PatternMode.Pattern), async (channel, key) =>
            {
                if (key.ToString().StartsWith("ReserveCart:"))
                {
                    var customerId = key.ToString().Replace("ReserveCart:", "");

                    using var scope = _serviceProvider.CreateScope();
                    var reserveCartCacheService = scope.ServiceProvider.GetRequiredService<IReserveCartCacheService>();

                    await reserveCartCacheService.HandleExpiredCartAsync(customerId);
                }
            });

            await Task.CompletedTask; // Ensures ExecuteAsync completes non-blocking
        }
    }
}
