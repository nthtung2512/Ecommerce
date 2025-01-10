using MealMate.Base;
using MealMate.Base.Extensions;
using MealMate.Base.Hub;
using MealMate.BLL.AutoMapperProfiles;
using MealMate.BLL.ExceptionHandler;
using MealMate.BLL.IServices;
using MealMate.BLL.IServices.auth;
using MealMate.BLL.IServices.Hubs;
using MealMate.BLL.IServices.Payment;
using MealMate.BLL.IServices.Redis;
using MealMate.BLL.IServices.Utility;
using MealMate.BLL.Services;
using MealMate.BLL.Services.auth;
using MealMate.BLL.Services.Hubs;
using MealMate.BLL.Services.Payment;
using MealMate.BLL.Services.Redis;
using MealMate.BLL.Services.Utility;
using MealMate.DAL.Utils.GuidUtil;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MealMate.BLL
{
    public class MealMateBLLModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            var config = services.GetConfiguration();
            services.AddAutoMapper(typeof(MealMateAutoMapperProfile));

            services.AddScoped<IApplicationUserAppService, ApplicationUserAppService>();
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IEmployeeAppService, EmployeeAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IBillPromotionAppService, BillPromotionAppService>();
            services.AddScoped<ICategoryPromotionAppService, CategoryPromotionAppService>();
            services.AddScoped<IProductPromotionAppService, ProductPromotionAppService>();
            services.AddScoped<ICustomerPromotionAppService, CustomerPromotionAppService>();
            services.AddScoped<IShipperAppService, ShipperAppService>();
            services.AddScoped<IStoreAppService, StoreAppService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IMapProductService, MapProductService>();

            services.AddScoped<IMomoService, MomoService>();
            services.AddScoped<IVnPayService, VnPayService>();

            services.AddSingleton(new GuidGenerator(SequentialGuidType.SequentialAsString));

            services.AddExceptionHandler<DomainExceptionHandler>();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IReserveCartCacheService, ReserveCartCacheService>();
            services.AddScoped<IReserveCartItemCacheService, ReserveCartItemCacheService>();

            services.AddSignalR();

            services.AddTransient<IHubContextWrapper<IProductHubClient>, ProductHubWrapper>();

            services.AddHostedService<RedisExpirationSubscriber>();


            /*services.AddScoped<IProductHubClient, ProductHubClient>();*/

            // Schedule job
            /*services.AddHostedService<PromotionCleanupService>();*/
        }
    }
}
