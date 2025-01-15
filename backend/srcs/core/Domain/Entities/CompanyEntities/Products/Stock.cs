using Domain.Enums;

namespace Domain.Entities.CompanyEntities.Products;

public sealed class Stock {
	public decimal                 StockQuantity { get; set; }
	public ProductUnitOfMeasureEnum UnitOfMeasure { get; set; }
}