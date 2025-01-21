using MediatR;
using TS.Result;

namespace Application.Features.Commands.CashRegisters.CashRegisterDelete;

public sealed record CashRegisterDeleteRequest(
	Guid Id) : IRequest<Result<string>>;