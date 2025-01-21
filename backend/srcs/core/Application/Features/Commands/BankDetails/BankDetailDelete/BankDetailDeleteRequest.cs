using MediatR;
using TS.Result;

namespace Application.Features.Commands.BankDetails.BankDetailDelete;

public sealed record BankDetailDeleteRequest(
	Guid Id) : IRequest<Result<string>>;