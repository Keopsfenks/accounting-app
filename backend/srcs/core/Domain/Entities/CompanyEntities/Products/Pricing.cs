namespace Domain.Entities.CompanyEntities.Products;

public sealed class Pricing {

	public decimal Quantity      { get; set; }
	public decimal UnitPrice    { get; set; }
	public decimal TaxRate      { get; set; }
	public decimal TotalPrice   { get; set; }
	public decimal  DiscountRate { get; set; }
}