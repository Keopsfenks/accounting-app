using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Contexts.ApplicationDb;

internal sealed class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid> {
	public AppDbContext(DbContextOptions options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder builder) {
		builder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);

		builder.Ignore<IdentityUserLogin<Guid>>();
		builder.Ignore<IdentityRoleClaim<Guid>>();
		builder.Ignore<IdentityUserToken<Guid>>();
		builder.Ignore<IdentityUserRole<Guid>>();
		builder.Ignore<IdentityUserClaim<Guid>>();
	}
}