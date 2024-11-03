using Microsoft.Extensions.DependencyInjection;

namespace MealMate.Base
{
    public abstract class Module
    {
        public abstract void ConfigureService(IServiceCollection services);
    }
}
