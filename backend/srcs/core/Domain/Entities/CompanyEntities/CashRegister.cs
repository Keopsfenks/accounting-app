using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities.CompanyEntities;

public sealed class CashRegister : BaseEntity {
	public string           Name             { get; set; } = string.Empty;
	public CurrencyTypeEnum CurrencyType     { get; set; } = CurrencyTypeEnum.TL;
	public decimal          DepositAmount    { get; set; }
	public decimal          WithdrawalAmount { get; set; }
	public decimal          BalanceAmount    { get; set; }
	public List<CashRegisterDetail>? Details { get; set; }
}