using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Entities.CompanyEntities.Products;
using Domain.Enums;

namespace Domain.Entities.CompanyEntities;

public sealed class CustomerDetail : BaseEntity {
	public Dictionary<string, Guid>  Processor   { get; set; }
	public Dictionary<string, Guid?> Opposite    { get; set; }
	public string                    Name        { get; set; }
	public string                    Description { get; set; }
	public string                    IssueDate   { get; set; }
	public string?                   DueDate     { get; set; }

	public decimal DepositAmount    { get; set; }
	public decimal WithdrawalAmount { get; set; }
	public decimal TotalAmount      { get; set; }

	public List<ProductDetail> Products     { get; set; }
	public List<CashProceeds>  CashProceeds { get; set; }

	public PaymentEnum         Payment   { get; set; } = PaymentEnum.InCash;
	public OperationTypeEnum   Operation { get; set; } = OperationTypeEnum.Sales;
	public StatusEnum          Status    { get; set; } = StatusEnum.Sent;

	public Guid  CustomerId      { get; set; }
	[JsonIgnore]
	public Customer Customer   { get;         set; }
}