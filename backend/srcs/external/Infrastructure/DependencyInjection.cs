using System.Reflection;
using Infrastructure.Authentication.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Infrastructure;

public static class DependencyInjection {
	public static IServiceCollection
		AddInfrastructure(this IServiceCollection services, IConfiguration configurations) {
		services.Configure<JwtOptions>(configurations.GetSection("Jwt"));
		services.ConfigureOptions<JwtTokenOptionsSetup>();
		services.AddAuthentication(options => {
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer();
		services.AddAuthorization();
		
		services.Scan(action => {
			action
				.FromAssemblies(Assembly.GetExecutingAssembly())
				.AddClasses(publicOnly: false)
				.UsingRegistrationStrategy(RegistrationStrategy.Skip)
				.AsMatchingInterface()
				.AsImplementedInterfaces()
				.WithScopedLifetime();
		});
		return services;
	}
}