using Application.Features.Commands.CashRegisterDetails.CashRegisterDetailCreate;
using Application.Features.Commands.CashRegisterDetails.CashRegisterDetailDelete;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.CashRegisterDetails.CashRegisterDetailUpdate;

public sealed record CashRegisterDetailUpdateRequest(
	Guid     Id,
	Guid     CashRegisterId,
	DateOnly Date,
	int      Type,
	decimal  Amount,
	Guid?    CashRegisterDetailId,
	Guid?	 OppositeBankId,
	string   Description) : IRequest<Result<string>>;



internal sealed record CashRegisterDetailUpdateHandler(
	IMediator mediator) : IRequestHandler<CashRegisterDetailUpdateRequest, Result<string>> {
	public async Task<Result<string>> Handle(CashRegisterDetailUpdateRequest request, CancellationToken cancellationToken) {
		Result<string> deleteResult = await mediator.Send(new CashRegisterDetailDeleteRequest(request.Id), cancellationToken);

		if (deleteResult.IsSuccessful is false)
			return deleteResult;

		Result<string> createResult = await mediator.Send(new CashRegisterDetailCreateRequest(request.CashRegisterId, request.Date, request.Type, request.Amount, request.CashRegisterDetailId, request.OppositeBankId, request.Description), cancellationToken);

		if (createResult.IsSuccessful is false)
			return createResult;

		return "Cash register detail updated successfully";
	}
}