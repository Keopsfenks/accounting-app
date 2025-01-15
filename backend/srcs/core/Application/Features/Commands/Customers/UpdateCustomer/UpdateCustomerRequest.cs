using Application.Features.Commands.Customers.CreateCustomer;
using Application.Features.Commands.Customers.DeleteCustomer;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Customers.UpdateCustomer;

public sealed record UpdateCustomerRequest(
	Guid    Id,
	string  Name,
	int     CustomerType,
	string? Description,
	string? Email,
	string? Phone,
	string? Adress,
	string? City,
	string? Town,
	string? Country,
	string? ZipCode,
	string? TaxId,
	string? TaxDepartment) : IRequest<Result<string>>;


internal sealed record UpdateCustomerHandler(
	IMediator mediator) : IRequestHandler<UpdateCustomerRequest, Result<string>> {
	public async Task<Result<string>> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken) {
		Result<string> deleteResult = await mediator.Send(new DeleteCustomerRequest(request.Id), cancellationToken);

		if (deleteResult.IsSuccessful is false) {
			return deleteResult;
		}

		Result<string> createResult = await mediator.Send(new CreateCustomerRequest(
			request.Name,
			request.CustomerType,
			request.Description,
			request.Email,
			request.Phone,
			request.Adress,
			request.City,
			request.Town,
			request.Country,
			request.ZipCode,
			request.TaxId,
			request.TaxDepartment
		), cancellationToken);

		if (createResult.IsSuccessful is false) {
			return createResult;
		}

		return "Customer updated successfully";
	}
}