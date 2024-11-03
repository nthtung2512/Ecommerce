using Microsoft.Extensions.DependencyInjection;

namespace MealMate.Base.Extensions
{
    internal static class ServiceDescriptorExtensions
    {
        public static object? NormalizedImplementationInstance(this ServiceDescriptor descriptor) =>
            descriptor.IsKeyedService
                ? descriptor.KeyedImplementationInstance
                : descriptor.ImplementationInstance;
    }
}
