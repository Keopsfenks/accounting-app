using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations.ApplicationDb;

internal sealed class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUsers> {
	public void Configure(EntityTypeBuilder<CompanyUsers> builder) {
		builder.ToTable("CompanyUsers");

		builder.HasKey(x => new { x.CompanyId, x.UserId, x.RoleId });

		builder.HasOne(c => c.Company)
			   .WithMany(c => c.UserRoles) // Company navigation property'sini belirtelim
			   .HasForeignKey(c => c.CompanyId)
			   .OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(c => c.User)
			   .WithMany(u => u.UserRoles)
			   .HasForeignKey(c => c.UserId)
			   .OnDelete(DeleteBehavior.Cascade);
	}
}