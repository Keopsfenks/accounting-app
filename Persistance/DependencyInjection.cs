using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Contexts.ApplicationDb;
using Scrutor;

namespace Persistance;

public static class DependencyInjection {
	public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration) {
		services.AddDbContext<AppDbContext>(options => {
			options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
		});

		services
			.AddIdentityCore<AppUser>(cfr => {
				cfr.Password.RequiredLength         = 1;
				cfr.Password.RequireNonAlphanumeric = false;
				cfr.Password.RequireUppercase       = false;
				cfr.Password.RequireLowercase       = false;
				cfr.Password.RequireDigit           = false;
				cfr.SignIn.RequireConfirmedEmail    = true;
				cfr.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(5);
				cfr.Lockout.MaxFailedAccessAttempts = 3;
				cfr.Lockout.AllowedForNewUsers      = true;
			})
			.AddEntityFrameworkStores<AppDbContext>();


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