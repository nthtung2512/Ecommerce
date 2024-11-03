using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MealMate.Base.Extensions
{
    public static class ServiceCollectionConfigurationExtensions
    {
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.GetConfigurationOrNull()
                ?? throw new Exception(
                    "Could not find an implementation of "
                        + typeof(IConfiguration).AssemblyQualifiedName
                        + " in the service collection."
                );
        }

        private static IConfiguration? GetConfigurationOrNull(this IServiceCollection services)
        {
            var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();
            if (hostBuilderContext?.Configuration != null)
            {
                return hostBuilderContext.Configuration as IConfigurationRoot;
            }

            return services.GetSingletonInstanceOrNull<IConfiguration>();
        }

        private static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T?)
                services
                    .FirstOrDefault(d => d.ServiceType == typeof(T))
                    ?.NormalizedImplementationInstance();
        }
    }
}
