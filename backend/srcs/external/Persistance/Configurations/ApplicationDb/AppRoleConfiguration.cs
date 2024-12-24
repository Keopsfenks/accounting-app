using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations.ApplicationDb;

internal sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole> {
	public void Configure(EntityTypeBuilder<AppRole> builder) {
		builder.ToTable("Roles");
		builder.HasKey(e => e.Id);
		builder.Property(e => e.Id).ValueGeneratedNever();
		builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
		builder.Property(e => e.NormalizedName).HasMaxLength(50).IsRequired();
		builder.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
	}
}