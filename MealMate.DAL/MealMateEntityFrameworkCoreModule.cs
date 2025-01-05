using MealMate.Base;
using MealMate.Base.Extensions;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.DAL.IRepositories;
using MealMate.DAL.IRepositories.auth;
using MealMate.DAL.IRepositories.UnitOfWork;
using MealMate.DAL.Repositories;
using MealMate.DAL.Repositories.auth;
using MealMate.DAL.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Identity;
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
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBillPromotionRepository, BillPromotionRepository>();
            services.AddScoped<IProductPromotionRepository, ProductPromotionRepository>();
            services.AddScoped<ICategoryPromotionRepository, CategoryPromotionRepository>();
            services.AddScoped<ICustomerPromotionRepository, CustomerPromotionRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IAtRepository, AtRepository>();
            services.AddScoped<IShipperRepository, ShipperRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<PasswordHasher<ApplicationUser>>();


            services.AddTransient<Seed>();
        }
    }
}
