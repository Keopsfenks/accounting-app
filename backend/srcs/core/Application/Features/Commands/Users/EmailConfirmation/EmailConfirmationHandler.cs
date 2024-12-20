using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace Application.Features.Commands.Users.EmailConfirmation;

public record EmailConfirmationHandler(
	UserManager<AppUser> userManager) : IRequestHandler<EmailConfirmationRequest, Result<string>> {
	public async Task<Result<string>> Handle(EmailConfirmationRequest request, CancellationToken cancellationToken) {
		AppUser? user = await userManager.FindByEmailAsync(request.Email);
		
		if (user == null) {
			return Result<string>.Failure("Mail is not registered for any user.");
		}

		if (user.EmailConfirmed) {
			return Result<string>.Failure("Mail is already confirmed.");
		}

		user.EmailConfirmed = true;
		await userManager.UpdateAsync(user);
		
		return Result<string>.Succeed("Mail confirmed successfully.");
	}
}