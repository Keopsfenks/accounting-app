using MediatR;
using TS.Result;

namespace Application.Features.Commands.CashRegisterDetails.CashRegisterDetailCreate;

public sealed record CashRegisterDetailCreateRequest(
	Guid     CashRegisterId,
	DateOnly Date,
	int      Type,
	decimal  Amount,
	Guid?    CashRegisterDetailId,
	Guid?	 OppositeBankId,
	string   Description) : IRequest<Result<string>>;