using Application.Features.Commands.Banks.BankCreate;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Banks.BankDelete;

public sealed record BankDeleteRequest(
	Guid Id) : IRequest<Result<string>>;