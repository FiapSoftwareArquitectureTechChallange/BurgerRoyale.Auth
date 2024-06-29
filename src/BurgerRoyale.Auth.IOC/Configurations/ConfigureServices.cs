using BurgerRoyale.Auth.Application.Services;
using BurgerRoyale.Auth.Domain.Interface.Repositories;
using BurgerRoyale.Auth.Domain.Interface.Services;
using BurgerRoyale.Auth.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Auth.IOC.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureServices
    {
        public static void Register
        (
            IServiceCollection services
        )
        {
            #region Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();

            #endregion Services

            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();

            #endregion Repositories
        }
    }
}