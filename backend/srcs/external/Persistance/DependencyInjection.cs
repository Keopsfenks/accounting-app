using System.Reflection;
using Domain.Entities;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Contexts;
using Persistance.Contexts.ApplicationDb;
using Persistance.Services;
using Scrutor;

namespace Persistance;

public static class DependencyInjection {
	public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration) {

		services.AddScoped<CompanyDbContext>();

		services.AddDbContext<AppDbContext>(options => {
			options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
		});

		services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<AppDbContext>());
		services.AddScoped<IUnitOfWorkCompany>(srv => srv.GetRequiredService<CompanyDbContext>());

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
		   .AddRoles<AppRole>()
		   .AddEntityFrameworkStores<AppDbContext>()
		   .AddRoleManager<RoleManager<AppRole>>();
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