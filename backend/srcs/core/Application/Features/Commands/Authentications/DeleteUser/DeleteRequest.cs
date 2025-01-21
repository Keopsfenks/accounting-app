using MediatR;
using TS.Result;

namespace Application.Features.Commands.Users.DeleteUser;

public sealed record DeleteRequest(
		Guid Id
) : IRequest<Result<string>>;