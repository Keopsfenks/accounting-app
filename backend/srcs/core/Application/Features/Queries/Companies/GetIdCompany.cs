using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Companies;

public sealed record GetIdCompanyResponse(
	string Id,
	string Name,
	string Address,
	string TaxId,
	string TaxDepartment
);

public sealed record GetIdCompany : IRequest<Result<GetIdCompanyResponse>> {
	public string? companyId { get; set; }
}

internal sealed record GetIdCompanyHandler(
	ICompanyRepository companyRepository,
	RoleManager<AppRole> roleManager) : IRequestHandler<GetIdCompany, Result<GetIdCompanyResponse>> {
	public async Task<Result<GetIdCompanyResponse>> Handle(GetIdCompany request, CancellationToken cancellationToken) {
		string? companyId = request.companyId;

		Company? company = await companyRepository.FirstOrDefaultAsync(c => c.Id.ToString() == request.companyId);

		if (company is null)
			return (500, "Company not found");

		return (new GetIdCompanyResponse(
			company.Id.ToString(),
			company.Name,
			company.Address,
			company.TaxId,
			company.TaxDepartment
		));
	}
}