using System.Reflection;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Authentication.Options;
using Infrastructure.Companies.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
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

		services.AddScoped<IAsyncAuthorizationFilter, CompanyAuthorizationFilter>(provider =>
		{

			return new CompanyAuthorizationFilter(provider.GetRequiredService<ICompanyUserRepository>(), "", provider.GetRequiredService<RoleManager<AppRole>>());
		});

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