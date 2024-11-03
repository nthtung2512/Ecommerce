﻿using MealMate.Base;
using MealMate.BLL.AutoMapperProfiles;
using MealMate.BLL.ExceptionHandler;
using MealMate.BLL.IServices;
using MealMate.BLL.Services;
using MealMate.DAL.Utils.GuidUtil;
using Microsoft.Extensions.DependencyInjection;

namespace MealMate.BLL
{
    public class MealMateBLLModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MealMateAutoMapperProfile));

            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IEmployeeAppService, EmployeeAppService>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IBillPromotionAppService, BillPromotionAppService>();
            services.AddScoped<ICategoryPromotionAppService, CategoryPromotionAppService>();
            services.AddScoped<IProductPromotionAppService, ProductPromotionAppService>();
            services.AddScoped<IShipperAppService, ShipperAppService>();
            services.AddScoped<IStoreAppService, StoreAppService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddSingleton(new GuidGenerator(SequentialGuidType.SequentialAsString));

            services.AddExceptionHandler<DomainExceptionHandler>();
        }
    }
}