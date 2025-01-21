using MediatR;
using TS.Result;

namespace Application.Features.Commands.CashProceeds.UpdateCashProceeds;

public sealed record UpdateCashProceedsRequest() : IRequest<Result<string>>;




internal sealed record UpdateCashProceedsHandler() : IRequestHandler<UpdateCashProceedsRequest, Result<string>> {
	public async Task<Result<string>> Handle(UpdateCashProceedsRequest request, CancellationToken cancellationToken) {

		return ("Cash Proceeds updated successfully");
	}
}