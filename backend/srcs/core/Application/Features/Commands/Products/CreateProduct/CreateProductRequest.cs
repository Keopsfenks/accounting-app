using Domain.Entities.CompanyEntities.Products;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Products.CreateProduct;

public sealed record CreateProductRequest(
	string Name,
	string Description,
	string IsActive,
	int   UnitOfMeasure,
	Guid?  Category) : IRequest<Result<string>>;