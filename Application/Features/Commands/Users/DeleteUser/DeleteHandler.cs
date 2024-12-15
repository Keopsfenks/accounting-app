using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace Application.Features.Commands.Users.DeleteUser;

internal sealed record DeleteHandler(
	UserManager<AppUser> userManager) : IRequestHandler<DeleteRequest, Result<DeleteResponse>> {
	
	public async Task<Result<DeleteResponse>> Handle(DeleteRequest request, CancellationToken cancellationToken) {
		AppUser user = await userManager.FindByIdAsync(request.Id.ToString());

		if (user == null) {
			return Result<DeleteResponse>.Failure("User not found");
		}
		
		user.IsDeleted = true;

		IdentityResult result = await userManager.UpdateAsync(user);
		
		if (!result.Succeeded) {
			return Result<DeleteResponse>.Failure(result.Errors.Select(s => s.Description).ToList());
		}
		
		return Result<DeleteResponse>.Succeed(new DeleteResponse("User deleted successfully"));
	}
}