using BurgerRoyale.Auth.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Auth.IOC.Configurations
{
	[ExcludeFromCodeCoverage]
	public static class ConfigureHealthChecks
	{
		public static void Register
		(
			IServiceCollection services
		)
		{
			services
				.AddHealthChecks()
				.AddDbContextCheck<ApplicationDbContext>();
		}
	}
}
