using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace Application.Features.Commands.Users.EmailConfirmation;

public record EmailConfirmationHandler(
	UserManager<AppUser> userManager) : IRequestHandler<EmailConfirmationRequest, Result<EmailConfirmationResponse>> {
	public async Task<Result<EmailConfirmationResponse>> Handle(EmailConfirmationRequest request, CancellationToken cancellationToken) {
		AppUser? user = await userManager.FindByEmailAsync(request.Email);
		
		if (user == null) {
			return Result<EmailConfirmationResponse>.Failure("Mail is not registered for any user.");
		}

		if (user.EmailConfirmed) {
			return Result<EmailConfirmationResponse>.Failure("Mail is already confirmed.");
		}

		user.EmailConfirmed = true;
		await userManager.UpdateAsync(user);
		
		return Result<EmailConfirmationResponse>.Succeed(new EmailConfirmationResponse("Mail confirmed successfully."));
	}
}