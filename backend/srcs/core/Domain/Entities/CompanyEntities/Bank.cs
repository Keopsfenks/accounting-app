using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities.CompanyEntities;

public sealed class Bank : BaseEntity {
	public string           Name           { get; set; }
	public string           Iban           { get; set; }
	public CurrencyTypeEnum CurrencyType   { get; set; } = CurrencyTypeEnum.TL;
	public decimal          DepositAmount  { get; set; }
	public decimal          WithdrawAmount { get; set; }
	public decimal          Balance        { get; set; }

	public ICollection<BankDetail> Details { get; set; }
}