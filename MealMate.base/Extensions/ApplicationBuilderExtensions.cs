using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MealMate.Base.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddModules<TModule>(this IHostApplicationBuilder builder)
            where TModule : Module, new()
        {
            ResolveDependentModules(typeof(TModule), builder.Services);
        }

        private static void ResolveDependentModules(Type currentType, IServiceCollection services)
        {
            var dependsOns = currentType
                .GetCustomAttributes(typeof(DependsOnAttribute), false)
                .FirstOrDefault();

            if (dependsOns is DependsOnAttribute dependants)
            {
                foreach (var dependType in dependants.DependedTypes)
                {
                    if (dependType.IsSubclassOf(typeof(Module)))
                    {
                        ResolveDependentModules(dependType, services);
                    }
                }
            }

            var currentInstance = Activator.CreateInstance(currentType) as Module;
            currentInstance?.ConfigureService(services);
        }
    }
}
