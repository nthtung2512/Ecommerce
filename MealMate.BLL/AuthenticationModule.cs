using MealMate.Base;
using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MealMate.BLL
{
    public class AuthenticationModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddAuthorization();

            // Identity services
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                // Allow duplicate emails
                options.User.RequireUniqueEmail = true;

                // Allow spaces, dots, hyphens, etc. in usernames
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";

                // Password requirements
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1; // Minimum length set to 1
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.AllowedForNewUsers = false; // Disables lockout for new users
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Zero;
                options.Lockout.MaxFailedAccessAttempts = int.MaxValue; // Effectively disables lockout

                // Sign-in settings
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
                    .AddEntityFrameworkStores<MealMateDbContext>()
                    .AddDefaultTokenProviders()
                    .AddDefaultUI();

            // Add cookie authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Change to cookie
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(6);
                options.SlidingExpiration = true;
                options.Cookie.Name = "CredentialCookie";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // For HTTPS
                options.Cookie.HttpOnly = true; // Prevent JavaScript access
            });
            /*services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Use JWT for challenge
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(6); // Cookie expires in 6 hours
                options.SlidingExpiration = true; // Refresh expiration time on activity
                options.Cookie.Name = "CredentialCookie";
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });*/
        }
    }

}
