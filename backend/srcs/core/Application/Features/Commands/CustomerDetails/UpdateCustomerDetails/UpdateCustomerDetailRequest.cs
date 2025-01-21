using MediatR;
using TS.Result;

namespace Application.Features.Commands.CustomerDetails.UpdateCustomerDetails;

public sealed record UpdateCustomerDetailRequest : IRequest<Result<string>> {
	
}


internal sealed record UpdateCustomerDetailHandler : IRequestHandler<UpdateCustomerDetailRequest, Result<string>> {
	public async Task<Result<string>> Handle(UpdateCustomerDetailRequest request, CancellationToken cancellationToken) {
		return ("Sales transaction is deleted successfully");
	}
}