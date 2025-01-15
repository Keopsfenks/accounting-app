using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.CashRegisters;

public sealed record GetAllCashRegister() : IRequest<Result<List<CashRegister>>> {
	public int PageNumber { get; set; } = 0;
	public int PageSize   { get; set; } = 10;
}




internal sealed record GetAllCashRegisterHandler(
	ICashRegisterRepository cashRegisterRepository) : IRequestHandler<GetAllCashRegister, Result<List<CashRegister>>> {
	public async Task<Result<List<CashRegister>>> Handle(GetAllCashRegister request, CancellationToken cancellationToken) {
		int pageNumber = request.PageNumber;
		int pageSize   = request.PageSize;

		List<CashRegister> cashRegisters = await cashRegisterRepository.GetAll()
														 .OrderBy(cr => cr.Name)
														 .Skip(pageNumber * pageSize)
														 .Include(cr => cr.Details)
														 .Take(pageSize)
														 .ToListAsync(cancellationToken);
		return cashRegisters;
	}
}