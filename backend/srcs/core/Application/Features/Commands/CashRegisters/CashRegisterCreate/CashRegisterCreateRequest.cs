using MediatR;
using TS.Result;

namespace Application.Features.Commands.CashRegisters.CashRegisterCreate;

public sealed record CashRegisterCreateRequest(
	string Name,
	int CurrencyType) : IRequest<Result<string>>;