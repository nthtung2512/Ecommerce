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

            // Build the connection string from environment variables
            var host = Environment.GetEnvironmentVariable("POSTGRESQL_HOST");
            var port = Environment.GetEnvironmentVariable("POSTGRESQL_PORT");
            var database = Environment.GetEnvironmentVariable("POSTGRESQL_DATABASE");
            var username = Environment.GetEnvironmentVariable("POSTGRESQL_USERNAME");
            var password = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD");

            var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

            var builder = new DbContextOptionsBuilder<MealMateDbContext>().UseNpgsql(connectionString);

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
