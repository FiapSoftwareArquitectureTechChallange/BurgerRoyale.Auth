using BurgerRoyale.Auth.Domain.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BurgerRoyale.Auth.IOC.Configurations
{
    public static class ConfigureSecurity
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
