using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
	public void Configure(EntityTypeBuilder<Company> builder)
	{
		builder.ToTable("Companies");
		builder.Property(c => c.TaxId).HasColumnType("varchar(11)");
		builder.HasKey(c => c.Id);

		builder.OwnsOne(c => c.Database,builder => {
			builder.Property(property => property.Server).HasColumnName("Server");
			builder.Property(property => property.DatabaseName).HasColumnName("DatabaseName");
			builder.Property(property => property.UserId).HasColumnName("UserId");
			builder.Property(property => property.Password).HasColumnName("Password");
		});

		builder.HasMany(c => c.UserRoles)         // Bir şirketin birden fazla kullanıcısı olabilir
			   .WithOne(cu => cu.Company)         // Her CompanyUsers bir Company'e bağlıdır
			   .HasForeignKey(cu => cu.CompanyId) // Foreign key ilişkilendirmesi
			   .OnDelete(DeleteBehavior.Cascade); // Şirket silindiğinde ilişkili kullanıcılar da silinsin
	}
}
