using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Products;

public sealed record GetAllProducts() : IRequest<Result<List<Product>>> {
	public int PageSize { get; set; } = 10;
	public int PageNumber { get; set; } = 0;
}



internal sealed record GetAllProductsHandler(
	IProductRepository productRepository) : IRequestHandler<GetAllProducts, Result<List<Product>>> {
	public async Task<Result<List<Product>>> Handle(GetAllProducts request, CancellationToken cancellationToken) {
		int PageSize   = request.PageSize;
		int PageNumber = request.PageNumber;

		List<Product> products = await productRepository.GetAll()
														.OrderBy(p => p.Name)
														.Skip(PageSize * PageNumber)
														.Include(p => p.Operations)
														.Take(PageSize)
														.ToListAsync(cancellationToken);

		return products;
	}
}