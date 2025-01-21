using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Banks;

public sealed record GetAllBanks() : IRequest<Result<List<Bank>>> {
	public int PageSize { get; init; } = 10;
	public int PageNumber { get; init; } = 0;
};


internal sealed record GetAllBanksHandler(
	IBankRepository bankRepository) : IRequestHandler<GetAllBanks, Result<List<Bank>>> {
	public async Task<Result<List<Bank>>> Handle(GetAllBanks request, CancellationToken cancellationToken) {
		int PageSize = request.PageSize;
		int PageNumber = request.PageNumber;

		List<Bank> banks = await bankRepository.GetAll()
											   .OrderBy(b => b.Name)
											   .Skip(PageNumber * PageSize)
											   .Include(b => b.Details)
											   .Take(PageSize)
											   .ToListAsync(cancellationToken);
		return banks;
	}
}