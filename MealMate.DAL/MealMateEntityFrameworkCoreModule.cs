using MealMate.Base;
using MealMate.Base.Extensions;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MealMate.DAL
{
    public class MealMateEntityFrameworkCoreModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddDbContext<MealMateDbContext>(options =>
            {
                options.UseNpgsql(services.GetConfiguration().GetConnectionString("Default"));
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBillPromotionRepository, BillPromotionRepository>();
            services.AddScoped<IProductPromotionRepository, ProductPromotionRepository>();
            services.AddScoped<ICategoryPromotionRepository, CategoryPromotionRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IAtRepository, AtRepository>();
            services.AddScoped<IShipperRepository, ShipperRepository>();

            services.AddTransient<Seed>();
        }
    }
}
