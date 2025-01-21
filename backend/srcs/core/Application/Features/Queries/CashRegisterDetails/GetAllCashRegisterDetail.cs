using Application.Features.Queries.CashRegisters;
using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.CashRegisterDetails;

public sealed record GetAllCashRegisterDetail() : IRequest<Result<List<CashRegisterDetail>>> {
	public int PageNumber { get; set; } = 0;
	public int PageSize   { get; set; } = 10;
}




internal sealed record GetAllCashRegisterDetailHandler(
	ICashRegisterDetailRepository cashRegisterDetailRepository) : IRequestHandler<GetAllCashRegisterDetail, Result<List<CashRegisterDetail>>> {
	public async Task<Result<List<CashRegisterDetail>>> Handle(GetAllCashRegisterDetail request, CancellationToken cancellationToken) {
		int pageNumber = request.PageNumber;
		int pageSize   = request.PageSize;

		List<CashRegisterDetail> cashRegisterDetails = await cashRegisterDetailRepository.GetAll()
																	   .OrderBy(cr => cr.Date)
																	   .Skip(pageNumber * pageSize)
																	   .Take(pageSize)
																	   .ToListAsync(cancellationToken);
		return cashRegisterDetails;
	}
}