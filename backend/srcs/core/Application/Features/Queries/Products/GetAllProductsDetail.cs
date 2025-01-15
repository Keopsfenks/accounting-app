using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Products;

public sealed record GetAllProductsDetail() : IRequest<Result<List<ProductDetail>>> {
	public int PageSize   { get; set; } = 10;
	public int PageNumber { get; set; } = 0;
}



internal sealed record GetAllProductsDetailHandler(
	IProductDetailRepository productDetailRepository) : IRequestHandler<GetAllProductsDetail, Result<List<ProductDetail>>> {
	public async Task<Result<List<ProductDetail>>> Handle(GetAllProductsDetail request, CancellationToken cancellationToken) {
		int PageSize   = request.PageSize;
		int PageNumber = request.PageNumber;

		List<ProductDetail> productDetails = await productDetailRepository.GetAll()
														.OrderBy(p => p.Date)
														.Skip(PageSize * PageNumber)
														.Take(PageSize)
														.ToListAsync(cancellationToken);

		return productDetails;
	}
}