using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection {
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configurations) {
		services.AddAutoMapper(typeof(DependencyInjection).Assembly);

		services.AddMediatR(conf => {
			conf.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
		});
		
		return services;
	}
}