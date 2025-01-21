using MediatR;
using TS.Result;

namespace Application.Features.Commands.Banks.BankCreate;

public sealed record BankCreateRequest(
	string Name,
	string Iban,
	int CurrencyType) : IRequest<Result<string>>;