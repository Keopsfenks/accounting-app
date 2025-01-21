using Application.Features.Commands.Banks.BankCreate;
using Application.Features.Commands.Banks.BankDelete;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Banks.BankUpdate;

public sealed record BankUpdateRequest(
	Guid   Id,
	string Name,
	string Iban,
	int    CurrencyType) : IRequest<Result<string>>;



internal sealed record BankUpdateHandler(
	IMediator mediator) : IRequestHandler<BankUpdateRequest, Result<string>> {
	public async Task<Result<string>> Handle(BankUpdateRequest request, CancellationToken cancellationToken) {
		Result<string> deleteResult = await mediator.Send(new BankDeleteRequest(request.Id), cancellationToken);

		if (deleteResult.IsSuccessful is false)
			return deleteResult;

		Result<string> createResult = await mediator.Send(new BankCreateRequest(request.Name, request.Iban, request.CurrencyType), cancellationToken);

		if (createResult.IsSuccessful is false)
			return createResult;

		return "Bank updated successfully";
	}
}