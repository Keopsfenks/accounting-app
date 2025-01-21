using MediatR;
using TS.Result;

namespace Application.Features.Commands.CashRegisterDetails.CashRegisterDetailDelete;

public sealed record CashRegisterDetailDeleteRequest(Guid Id) : IRequest<Result<string>>;