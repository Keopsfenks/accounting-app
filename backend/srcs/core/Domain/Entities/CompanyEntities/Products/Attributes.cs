namespace Domain.Entities.CompanyEntities.Products;

public sealed class Attributes {

	public string?                     Brand                { get; set; }
	public string?                     Dimensions           { get; set; }
	public string?                     Weight               { get; set; }
	public Dictionary<string, string>? AdditionalAttributes { get; set; }
}