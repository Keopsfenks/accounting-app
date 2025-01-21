using Application.Features.Commands.Companies.CreateCompany;
using Application.Features.Commands.Companies.DeleteCompany;
using Domain.ValueObjects;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Companies.UpdateCompany;

public sealed record UpdateCompanyRequest(
	Guid	 Id,
	string   Name,
	string   Address,
	string   TaxDepartment,
	string   TaxId,
	Database Database) : IRequest<Result<string>>;


public sealed record UpdateCompanyHandler(
	IMediator mediator) : IRequestHandler<UpdateCompanyRequest, Result<string>> {
	public async Task<Result<string>> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken) {
		Result<string> deleteResult = await mediator.Send(new DeleteCompanyRequest(request.Id), cancellationToken);

		if (deleteResult.IsSuccessful is false)
			return deleteResult;

		Result<string> createResult = await mediator.Send(new CreateCompanyRequest(request.Name, request.Address, request.TaxDepartment, request.TaxId, request.Database), cancellationToken);

		if (createResult.IsSuccessful is false)
			return createResult;

		return "Company updated successfully";
	}
}