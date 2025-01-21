using System.Security.Claims;
using Application.Services.Companies;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using TS.Result;

namespace Application.Features.Commands.Companies.UpdateDatabase;

public sealed record UpdateCompanyDatabase() : IRequest<Result<string>>;




internal sealed record UpdateCompanyDatabaseHandler(
	IHttpContextAccessor httpContextAccessor,
	ICompanyRepository companyRepository,
	ICompanyService companyService) : IRequestHandler<UpdateCompanyDatabase, Result<string>> {
	public async Task<Result<string>> Handle(UpdateCompanyDatabase request, CancellationToken cancellationToken) {
		string? companyID = httpContextAccessor.HttpContext.User.FindFirstValue("CompanyId");

		if (string.IsNullOrEmpty(companyID)) {
			return (500, "Company not found");
		}

		Company? company = await companyRepository.FirstOrDefaultAsync(c => c.Id.ToString() == companyID, cancellationToken);

		if (company is null) {
			return (500, "Company not found");
		}

		companyService.MigrateCompanyDatabase(company);

		return "Company database updated successfully";
	}
}