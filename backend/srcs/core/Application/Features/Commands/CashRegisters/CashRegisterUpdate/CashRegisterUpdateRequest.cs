using Application.Features.Commands.CashRegisters.CashRegisterCreate;
using Application.Features.Commands.CashRegisters.CashRegisterDelete;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.CashRegisters.CashRegisterUpdate;

public  sealed record CashRegisterUpdateRequest(
	Guid   Id,
	string Name,
	int    CurrencyType) : IRequest<Result<string>>;


internal sealed record CashRegisterUpdateHandler(
	IMediator mediator) : IRequestHandler<CashRegisterUpdateRequest, Result<string>> {
	public async Task<Result<string>> Handle(CashRegisterUpdateRequest request, CancellationToken cancellationToken) {
		Result<string> deleteResult = await mediator.Send(new CashRegisterDeleteRequest(request.Id), cancellationToken);

		if (deleteResult.IsSuccessful is false)
			return deleteResult;

		Result<string> createResult = await mediator.Send(new CashRegisterCreateRequest(request.Name, request.CurrencyType), cancellationToken);

		if (createResult.IsSuccessful is false)
			return createResult;

		return "Cash register updated successfully";

	}
}