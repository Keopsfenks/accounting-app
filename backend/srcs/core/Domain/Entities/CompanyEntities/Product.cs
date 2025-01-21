using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Entities.CompanyEntities.Products;
using Domain.Enums;

namespace Domain.Entities.CompanyEntities;

public sealed class Product : BaseEntity
{
	public string  Name        { get; set; }
	public string  Description { get; set; }
	public bool    IsActive    { get; set; } = true;

	public ProductUnitOfMeasureEnum UnitOfMeasure { get; set; } = ProductUnitOfMeasureEnum.Unit;
	public Guid?               CategoryId { get; set; }
	[JsonIgnore]
	public Category?           Category   { get;    set; }
	public List<ProductDetail> Operations    { get; set; }

}