using System.Text.Json.Serialization;
using Domain.Abstractions;

namespace Domain.Entities.CompanyEntities;

public sealed class BankDetail : BaseEntity {
	public Dictionary<string, Guid>  Processor        { get; set; }
	public string                    Date             { get; set; }
	public string                    Description      { get; set; } = string.Empty;
	public decimal                   DepositAmount    { get; set; }
	public decimal                   WithdrawalAmount { get; set; }
	public Dictionary<string, Guid?> Opposite         { get; set; }

	[JsonIgnore]
	public Bank Bank { get;                         set; }
	public Guid BankId                       { get; set; }
}