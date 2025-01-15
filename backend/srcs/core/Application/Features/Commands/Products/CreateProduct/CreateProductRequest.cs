using Domain.Entities.CompanyEntities.Products;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Products.CreateProduct;

public sealed record CreateProductRequest(
	string  Name,
	string  Description,
	string  IsActive,
	Guid?   Category,
	Attributes? Attributes,
	Metadata? Metadata,
	Stock? Stock) : IRequest<Result<string>>;