using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.CashProceeds;

public sealed record GetAllCashProceeds() : IRequest<Result<List<CashProceed>>> {
	public int PageNumber { get; set; } = 0;
	public int PageSize   { get; set; } = 10;
};


internal sealed record GetAllCashProceedsHandler(
	ICashProceedRepository cashProceedRepository) : IRequestHandler<GetAllCashProceeds, Result<List<CashProceed>>> {
	public async Task<Result<List<CashProceed>>> Handle(GetAllCashProceeds request, CancellationToken cancellationToken) {
		int pageNumber = request.PageNumber;
		int pageSize   = request.PageSize;

		List<CashProceed> cashProceeds = await cashProceedRepository.GetAll()
														 .OrderBy(cp => cp.Description)
														 .Skip(pageNumber * pageSize)
														 .Take(pageSize)
														 .ToListAsync(cancellationToken);
		return cashProceeds;
	}
}