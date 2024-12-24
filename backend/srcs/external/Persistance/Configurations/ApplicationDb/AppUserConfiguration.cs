using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations.ApplicationDb;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser> {
	public void Configure(EntityTypeBuilder<AppUser> builder) {
		builder.ToTable("Users");
		builder.HasKey(p => p.Id);
		builder.HasQueryFilter(x => !x.IsDeleted);
		builder.Property(p => p.FirstName).HasColumnType("varchar(50)");
		builder.Property(p => p.LastName).HasColumnType("varchar(50)");

		builder.HasMany(p => p.UserRoles)
			   .WithOne(cu => cu.User)
			   .HasForeignKey(cu => cu.UserId)
			   .OnDelete(DeleteBehavior.Cascade);
	}
}