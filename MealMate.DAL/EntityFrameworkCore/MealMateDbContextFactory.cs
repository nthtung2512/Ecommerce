using MealMate.PL.Environment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace MealMate.DAL.EntityFrameworkCore
{
    public class MealmateDbContextFactory : IDesignTimeDbContextFactory<MealMateDbContext>
    {
        public MealMateDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            Console.WriteLine(configuration.GetConnectionString("Default"));

            var builder = new DbContextOptionsBuilder<MealMateDbContext>().UseNpgsql(
                 configuration.GetConnectionString("Default")
             );


            return new MealMateDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var env =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToString()
                ?? SelectedEnvironment.Value.ToString();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{env}.json", true);

            return builder.Build();
        }
    }
}
