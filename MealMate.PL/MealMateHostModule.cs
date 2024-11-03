﻿using MealMate.Base;
using MealMate.Base.Extensions;
using MealMate.BLL;
using MealMate.DAL;
using Microsoft.OpenApi.Models;

namespace MealMate.PL
{
    [DependsOn(
    [
        typeof(MealMateBLLModule),
        typeof(MealMateEntityModule),
        typeof(MealMateEntityFrameworkCoreModule),
        typeof(AuthenticationModule)
    ]
)]
    public class MealMateHostModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            var configuration = services.GetConfiguration();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy
                        .WithOrigins(
                            configuration
                                .GetValue<string>("App:CorsOrigins")!
                                .Split(";", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                );
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

        }
    }

}