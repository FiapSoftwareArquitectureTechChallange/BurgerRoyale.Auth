using BurgerRoyale.Auth.Domain.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Auth.IOC.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureOptions
    {
        public static void Register
        (
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<JwtConfiguration>
            (
                options => configuration.GetSection("Jwt").Bind(options)
            );
        }
    }
}
