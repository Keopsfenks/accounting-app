using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Companies;

public sealed record GetUserToCompanies : IRequest<Result<List<CompanyUsers>>> {
	public int pageNumber { get; set; } = 0;
	public int pageSize { get; set; } = 10;
}


internal sealed record GetUserToCompaniesHandler(
	ICompanyUserRepository companyUserRepository,
	IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetUserToCompanies, Result<List<CompanyUsers>>> {
	public async Task<Result<List<CompanyUsers>>> Handle(GetUserToCompanies request, CancellationToken cancellationToken) {
		int pageNumber = request.pageNumber;
		int pageSize = request.pageSize;

		string? userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

		if (string.IsNullOrEmpty(userId))
			return Result<List<CompanyUsers>>.Failure("User not found");

		List<CompanyUsers> companyUsersList = await companyUserRepository.GetAll()
													.Where(c => c.UserId.ToString() == userId)
													.OrderBy(cu => cu.CreatedAt)
													.Skip(pageNumber * pageSize)
													.Take(pageSize)
													.ToListAsync(cancellationToken);

		return companyUsersList;
	}
}