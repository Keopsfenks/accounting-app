using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities.CompanyEntities;

public sealed class CashProceed : BaseEntity {
	public string            Description { get; set; }
	public string            IssueDate   { get; set; }
	public decimal           Amount      { get; set; }
	public PaymentTypeEnum   PaymentType { get; set; } = PaymentTypeEnum.Cash;
	public OperationTypeEnum Operation   { get; set; } = OperationTypeEnum.CashProceeds;
	public Cheque?           Cheque      { get; set; }

	public Guid? InvoiceId        { get; set; }
	public Guid? CustomerDetailId { get; set; }

	[JsonIgnore]
	public Invoice?        Invoice        { get; set; }
	[JsonIgnore]
	public CustomerDetail? CustomerDetail { get; set; }
}