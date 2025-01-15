using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Entities.CompanyEntities.Products;
using Domain.Enums;

namespace Domain.Entities.CompanyEntities;


public sealed class ProductDetail : BaseEntity {
	public Dictionary<string, Guid> Processor   { get; set; }

	public string                   Description { get; set; } = string.Empty;
	public string                   Date        { get; set; }
	public OperationTypeEnum        Type        { get; set; } = OperationTypeEnum.Sales;
	public Pricing                  Pricing     { get; set; }

	public Guid            ProductId        { get; set; }
	public Guid?           InvoiceId        { get; set; }
	public Guid?           CustomerDetailId { get; set; }
	[JsonIgnore]
	public Product         Product          { get; set; }
	[JsonIgnore]
	public Invoice?        Invoice          { get; set; }
	[JsonIgnore]
	public CustomerDetail? CustomerDetail   { get; set; }

}