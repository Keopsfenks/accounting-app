using Application.Features.Commands.BankDetails.BankDetailCreate;
using Application.Features.Commands.BankDetails.BankDetailDelete;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.BankDetails.BankDetailUpdate;

public sealed record BankDetailUpdateRequest(
	Guid     Id,
	Guid     BankId,
	DateOnly Date,
	int      Type,
	decimal  Amount,
	Guid?    OppositeBankId,
	Guid?	 OppositeCashRegisterId,
	string   Description) : IRequest<Result<string>>;



internal sealed record BankDetailUpdateHandler(
	IMediator mediator) : IRequestHandler<BankDetailUpdateRequest, Result<string>> {
	public async Task<Result<string>> Handle(BankDetailUpdateRequest request, CancellationToken cancellationToken) {

		Result<string> deleteResult = await mediator.Send(new BankDetailDeleteRequest(request.Id), cancellationToken);

		if (deleteResult.IsSuccessful is false)
			return deleteResult;

		Result<string> createResult = await mediator.Send(new BankDetailCreateRequest(request.BankId, request.Date, request.Type, request.Amount, request.OppositeBankId, request.OppositeCashRegisterId, request.Description), cancellationToken);

		if (createResult.IsSuccessful is false)
			return createResult;

		return "Bank detail updated successfully";
	}
}