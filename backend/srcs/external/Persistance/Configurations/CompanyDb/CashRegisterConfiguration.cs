using Domain.Entities.CompanyEntities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations.CompanyDb;

internal sealed class CashRegisterConfiguration : IEntityTypeConfiguration<CashRegister> {
	public void Configure(EntityTypeBuilder<CashRegister> builder) {
		builder.Property(p => p.DepositAmount).HasColumnType("money");
		builder.Property(p => p.WithdrawalAmount).HasColumnType("money");
		builder.Property(p => p.BalanceAmount).HasColumnType("money");
		builder.Property(p => p.CurrencyType)
			   .HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
	}
}