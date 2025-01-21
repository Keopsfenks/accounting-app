using Domain.Entities.CompanyEntities.Products;

namespace Domain.ValueObjects;

public sealed record ProductInformation(
	int Type,
	Guid ProductId,
	Pricing Pricing);