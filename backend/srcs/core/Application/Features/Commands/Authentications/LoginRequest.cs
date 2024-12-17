using MediatR;
using TS.Result;

namespace Application.Features.Commands.Authentication;

public sealed record LoginRequest(
	string EmailOrUsername,
	string Password) : IRequest<Result<LoginResponse>>;