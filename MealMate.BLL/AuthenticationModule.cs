using MealMate.Base;
using MealMate.DAL.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MealMate.BLL
{
    public class AuthenticationModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddIdentityApiEndpoints<IdentityUser>()
                    .AddEntityFrameworkStores<MealMateDbContext>();
        }
    }
}
