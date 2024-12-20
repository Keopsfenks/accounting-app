using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace Application.Features.Queries.Companies;

public sealed class GetAllCompanies() : IRequest<Result<string>> { }

internal sealed class GetAllCompaniesHandler() : IRequestHandler<GetAllCompanies, Result<string>> {
	public Task<Result<string>> Handle(GetAllCompanies request, CancellationToken cancellationToken) {
		throw new NotImplementedException();
	}
}