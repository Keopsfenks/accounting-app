using Application.Features.Commands.Users.DeleteUser;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace Application.Features.Commands.Authentications.DeleteUser;

internal sealed record DeleteHandler(
	UserManager<AppUser> userManager,
	IMediator mediator) : IRequestHandler<DeleteRequest, Result<string>> {
	
	public async Task<Result<string>> Handle(DeleteRequest request, CancellationToken cancellationToken) {
		AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());

		if (user == null) {
			return (500, "User not found");
		}

		IdentityResult result = await userManager.DeleteAsync(user);

		if (!result.Succeeded) {
			return (500, result.Errors.Select(s => s.Description).ToList());
		}

		return Result<string>.Succeed("User deleted successfully");
	}
}