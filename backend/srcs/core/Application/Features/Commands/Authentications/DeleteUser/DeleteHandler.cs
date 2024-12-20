using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace Application.Features.Commands.Users.DeleteUser;

internal sealed record DeleteHandler(
	UserManager<AppUser> userManager) : IRequestHandler<DeleteRequest, Result<string>> {
	
	public async Task<Result<string>> Handle(DeleteRequest request, CancellationToken cancellationToken) {
		AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());

		if (user == null) {
			return Result<string>.Failure("User not found");
		}
		
		user.IsDeleted = true;

		IdentityResult result = await userManager.UpdateAsync(user);
		
		if (!result.Succeeded) {
			return Result<string>.Failure(result.Errors.Select(s => s.Description).ToList());
		}
		
		return Result<string>.Succeed("User deleted successfully");
	}
}