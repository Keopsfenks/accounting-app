using Domain.Entities;
using GenericRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Application;

public static class DependencyInjection {
	public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configurations) {
		services.AddAutoMapper(typeof(DependencyInjection).Assembly);
		
		services.AddFluentEmail("info@emuhasebe.com").AddSmtpSender("localhost",2525);
		
		services.AddMediatR(conf => {
			conf.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
			conf.RegisterServicesFromAssemblies(typeof(AppUser).Assembly);
		});
		return services;
	}
}