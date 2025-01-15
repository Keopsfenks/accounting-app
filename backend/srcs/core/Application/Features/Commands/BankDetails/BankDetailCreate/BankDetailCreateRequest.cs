using MediatR;
using TS.Result;

namespace Application.Features.Commands.BankDetails.BankDetailCreate;

public sealed record BankDetailCreateRequest(
	Guid     BankId,
	DateOnly Date,
	int      Type,
	decimal  Amount,
	Guid?    OppositeBankId,
	Guid?    OppositeCashRegisterId,
	string   Description) : IRequest<Result<string>>;