using MediatR;
using TS.Result;

namespace Application.Features.Commands.Companies.UpdateCompany;

public sealed record UpdateCompanyRequest() : IRequest<Result<string>>;


public sealed record UpdateCompanyHandler() : IRequestHandler<UpdateCompanyRequest, Result<string>> {
	public Task<Result<string>> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken) {
		throw new NotImplementedException();
	}
}