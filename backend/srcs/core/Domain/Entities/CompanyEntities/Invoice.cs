using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities.CompanyEntities;

public sealed class Invoice : BaseEntity {
	public Dictionary<string, Guid> Processor     { get; set; }
	public string                   InvoiceNumber { get; set; } = string.Empty;
	public string                   Description   { get; set; } = string.Empty;
	public string                   IssueDate     { get; set; }
	public string?                  DueDate       { get; set; }
	public Guid?                    CompanyId     { get; set; }

	public List<ProductDetail> Products     { get; set; }
	public List<CashProceed>  CashProceeds { get; set; }

	public decimal          DepositAmount    { get; set; }
	public decimal          WithdrawalAmount { get; set; }
	public decimal          TotalAmount      { get; set; }

	public CurrencyTypeEnum  CurrencyType { get; set; } = CurrencyTypeEnum.TL;
	public PaymentEnum       Payment      { get; set; } = PaymentEnum.InCash;
	public OperationTypeEnum Operation    { get; set; } = OperationTypeEnum.Sales;
	public StatusEnum        Status       { get; set; } = StatusEnum.Sent;

	public Guid  CustomerId      { get; set; }
	[JsonIgnore]
	public Customer Customer   { get;         set; }

}