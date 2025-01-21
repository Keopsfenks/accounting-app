using MediatR;
using TS.Result;

namespace Application.Features.Commands.Companies.DeleteCompany;

public sealed record DeleteCompanyRequest(
	Guid companyId) : IRequest<Result<string>>;