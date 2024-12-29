using Domain.Entities;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Contexts.ApplicationDb;

public sealed class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IUnitOfWork
{
	public AppDbContext(DbContextOptions options) : base(options) { }

	public DbSet<Company>      Companies    { get; set; }
	public DbSet<CompanyUsers> CompanyUsers { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Ignore<IdentityUserLogin<Guid>>();
		builder.Ignore<IdentityRoleClaim<Guid>>();
		builder.Ignore<IdentityUserToken<Guid>>();
		builder.Ignore<IdentityUserRole<Guid>>();
		builder.Ignore<IdentityUserClaim<Guid>>();

		base.OnModelCreating(builder);

		builder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);
		SeedRoles(builder);
	}

	private void SeedRoles(ModelBuilder builder) {
		builder.Entity<AppRole>().HasData(
			new AppRole {Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString()}
		);
	}
}
