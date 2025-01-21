using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Products.DeleteProduct;

public sealed record DeleteProductRequest(
	Guid Id) : IRequest<Result<string>>;



internal sealed record DeleteProductHandler(
	IProductRepository productRepository,
	IUnitOfWorkCompany unitOfWorkCompany) : IRequestHandler<DeleteProductRequest, Result<string>> {
	public async Task<Result<string>> Handle(DeleteProductRequest request, CancellationToken cancellationToken) {
		Product? product = await productRepository.FirstOrDefaultAsync(p => p.Id == request.Id);

		if (product is null)
			return (500, "Product not found");

		productRepository.Delete(product);

		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return "Product deleted successfully";
	}
}