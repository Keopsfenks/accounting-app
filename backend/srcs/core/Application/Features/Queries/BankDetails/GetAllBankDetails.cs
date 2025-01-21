using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.BankDetails;

public sealed record GetAllBankDetails() : IRequest<Result<List<BankDetail>>> {
	public int pageSize { get; set; } = 10;
	public int pageNumber { get; set; } = 0;
}


internal sealed record GetAllBankDetailsHandler(
	IBankDetailRepository bankDetailRepository) : IRequestHandler<GetAllBankDetails, Result<List<BankDetail>>> {
	public async Task<Result<List<BankDetail>>> Handle(GetAllBankDetails request, CancellationToken cancellationToken) {
		int pageSize = request.pageSize;
		int pageNumber = request.pageNumber;

		// Get all bank details from database
		List<BankDetail> bankDetails = await bankDetailRepository.GetAll()
																 .OrderBy(cr => cr.Date)
																 .Skip(pageNumber * pageSize)
																 .Take(pageSize)
																 .ToListAsync(cancellationToken);

		return bankDetails;
	}
}