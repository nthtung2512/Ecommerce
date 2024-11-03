using FluentValidation;
using MealMate.Base;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.Entities.Shippers;
using MealMate.DAL.Entities.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace MealMate.DAL
{
    public class MealMateEntityModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddScoped<IValidator<Customer>, CustomerValidator>();
            services.AddScoped<IValidator<ShipperPhoneNo>, ShipperPhoneNoValidator>();
            services.AddScoped<IValidator<Product>, ProductValidator>();
        }
    }
}
