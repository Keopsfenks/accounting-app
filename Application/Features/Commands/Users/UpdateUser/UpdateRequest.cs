using MediatR;
using TS.Result;

namespace Application.Features.Commands.Users.UpdateUser;

public sealed record UpdateRequest(
	Guid   Id,
	string FirstName,
	string LastName,
	string UserName,
	string Email,
	string Password) : IRequest<Result<UpdateResponse>>;