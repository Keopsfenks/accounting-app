using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Companies;

public sealed record GetAllUserToCompanies() : IRequest<Result<List<CompanyUsers>>> {
	public int PageNumber { get; set; } = 0;
	public int PageSize   { get; set; } = 10;

	public string? userID { get; set; }
}
internal sealed record GetAllUserToCompanyHandler(
	ICompanyUserRepository companyUserRepository) : IRequestHandler<GetAllUserToCompanies, Result<List<CompanyUsers>>>
{

	public async Task<Result<List<CompanyUsers>>> Handle(GetAllUserToCompanies request, CancellationToken cancellationToken)
	{
		int     pageNumber = request.PageNumber;
		int     pageSize   = request.PageSize;
		string? userID     = request.userID;

		List<CompanyUsers> companyUsersList;
		if (userID is not null) {
			companyUsersList = await companyUserRepository.GetAll()
														  .Where(c => c.UserId.ToString() == userID)
														  .OrderBy(c => c.UserId)
														  .Skip(pageNumber * pageSize)
														  .Take(pageSize)
														  .ToListAsync(cancellationToken);

			return companyUsersList;
		}

		companyUsersList = await companyUserRepository.GetAll()
													  .OrderBy(c => c.CompanyId)
													  .Skip(pageNumber * pageSize)
													  .Take(pageSize)
													  .ToListAsync(cancellationToken);
		return companyUsersList;
	}
}