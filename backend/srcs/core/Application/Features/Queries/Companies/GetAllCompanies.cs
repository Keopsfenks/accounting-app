using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Dtos;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Companies;

public sealed record GetAllCompanies() : IRequest<Result<List<Company>>> {
	public int     PageNumber { get; set; } = 0;
	public int     PageSize   { get; set; } = 10;
	public string? Id         { get; set; }
}
internal sealed record GetAllCompaniesHandler(
	ICompanyRepository companyRepository) : IRequestHandler<GetAllCompanies, Result<List<Company>>>
{

	public async Task<Result<List<Company>>> Handle(GetAllCompanies request, CancellationToken cancellationToken)
	{
		int     pageNumber = request.PageNumber;
		int     pageSize   = request.PageSize;
		string? Id         = request.Id;

		List<Company> companies;

		if (Id is not null) {
			companies = await companyRepository.GetAll()
											   .Where(c => c.Id.ToString() == Id)
											   .OrderBy(c => c.Id)
											   .Skip(pageNumber * pageSize)
											   .Include(c => c.UserRoles)
											   .Take(pageSize)
											   .ToListAsync(cancellationToken);

			return companies;
		}
		companies = await companyRepository.GetAll()
										   .OrderBy(p => p.Name)
										   .Skip(pageNumber * pageSize)
										   .Include(c => c.UserRoles)
										   .Take(pageSize)
										   .ToListAsync(cancellationToken);

		return companies;
	}
}