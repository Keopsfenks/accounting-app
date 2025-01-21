using MediatR;
using TS.Result;

namespace Application.Features.Commands.Users.EmailConfirmation;

public record EmailConfirmationRequest(
	string Email) : IRequest<Result<string>>;