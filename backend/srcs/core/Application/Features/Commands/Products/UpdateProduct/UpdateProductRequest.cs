using Application.Features.Commands.Products.CreateProduct;
using Application.Features.Commands.Products.DeleteProduct;
using Domain.Entities.CompanyEntities.Products;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Products.UpdateProduct;

public sealed record UpdateProductRequest(
	Guid        Id,
	string      Name,
	string      Description,
	string      IsActive,
	decimal     Price,
	Guid?       Category,
	Attributes? Attributes,
	Metadata?   Metadata,
	Pricing?    Pricing,
	Stock?      Stock) : IRequest<Result<string>>;



internal sealed record UpdateProductHandler(
	IMediator mediator) : IRequestHandler<UpdateProductRequest, Result<string>> {
	public async Task<Result<string>> Handle(UpdateProductRequest request, CancellationToken cancellationToken) {
		Result<string> deleteResult = await mediator.Send(new DeleteProductRequest(request.Id), cancellationToken);

		if (deleteResult.IsSuccessful is false) {
			return deleteResult;
		}

		Result<string> createResult = await mediator.Send(new CreateProductRequest(
			request.Name,
			request.Description,
			request.IsActive,
			request.Category,
			request.Attributes,
			request.Metadata,
			request.Stock
		), cancellationToken);

		if (createResult.IsSuccessful is false) {
			return createResult;
		}

		return "Product updated successfully";
	}
}