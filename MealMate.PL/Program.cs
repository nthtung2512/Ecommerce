using MealMate.Base.Extensions;
using MealMate.BLL.Services.Hubs;
using MealMate.DAL.Entities.Payment;
using MealMate.DAL.EntityFrameworkCore;
using MealMate.PL;
using MealMate.PL.Environment;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;

Console.WriteLine("Configure Logging");

#region LoggerConfiguration
Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
    .WriteTo.Console()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs-.txt", rollingInterval: RollingInterval.Day))
    .CreateLogger();
#endregion

try
{
    DotNetEnv.Env.Load();
    var env =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToString()
        ?? SelectedEnvironment.Value.ToString();

    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env}.json", optional: true)
        .Build();

    Log.Information("Starting web host.");
    Console.WriteLine(
        $"Starting web host at {config.GetValue<string>("Kestrel:EndPoints:Http:Url")}"
    );
    Console.WriteLine($"Environment: {env}");

    var builder = WebApplication.CreateBuilder(args);

    // Connect MOMO API
    builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));

    builder.Host.UseSerilog();
    builder.AddModules<MealMateHostModule>();
    builder.Configuration.AddEnvironmentVariables();
    builder.Configuration.AddConfiguration(config);

    static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        string[] roleNames = { "Admin", "Customer", "StoreManager", "Shipper" };

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }
    }

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        string connection = builder.Configuration.GetConnectionString("Redis");
        options.Configuration = connection;
    });

    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));

    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        await SeedRolesAsync(scope.ServiceProvider);
    }

    using (var scope = app.Services.CreateScope())
    {
        var seedService = scope.ServiceProvider.GetRequiredService<Seed>();
        await seedService.Populate(env);  // This will now use the DbContext and UserManager within the scope
    }

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDefaultFiles();
    app.UseStaticFiles();

    /*    app.UseCors();*/
    // Configure the HTTP request pipeline.
    // app.UseCors("AllowSpecificOrigin");  // Apply CORS policy
    app.UseCors();
    app.UseRouting();
    app.UseHttpsRedirection();

    // empty Action<Configure> for unresolve error: https://github.com/dotnet/aspnetcore/issues/51888
    // remove it if the issue is closed
    app.UseExceptionHandler(configure => { });
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapDefaultControllerRoute();
    app.MapIdentityApi<IdentityUser>();

    app.MapHub<ProductHub>("/hubs/product");
    app.MapFallbackToFile("/index.html");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
    Console.WriteLine(ex.ToString());
}
finally
{
    Console.WriteLine($"Web host ended");
    Log.CloseAndFlush();
}

