using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Entities.CompanyEntities.Products;

namespace Domain.Entities.CompanyEntities;

public sealed class Product : BaseEntity
{
	public string  Name        { get; set; }
	public string  Description { get; set; }
	public bool    IsActive    { get; set; } = true;

	public decimal Deposit     { get; set; }
	public decimal Withdrawal  { get; set; }

	public Guid?               CategoryId { get; set; }
	[JsonIgnore]
	public Category?           Category   { get;    set; }
	public List<ProductDetail> Operations    { get; set; }

	public Attributes? Attributes { get; set; }
	public Metadata?   Metadata   { get; set; }
	public Stock?      Stock      { get; set; }

}