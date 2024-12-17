using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace Application.Features.Commands.Users.UpdateUser;

public record UpdateHandler(
	UserManager<AppUser> userManager,
	IMapper              mapper) : IRequestHandler<UpdateRequest, Result<UpdateResponse>> {

	public async Task<Result<UpdateResponse>> Handle(UpdateRequest request, CancellationToken cancellationToken) {
		AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());

		if (user is null) {
			return Result<UpdateResponse>.Failure("User not found.");
		}
		
		if (user.FirstName != request.FirstName) {
			user.FirstName = request.FirstName;
		}
		
		if (user.LastName != request.LastName) {
			user.LastName = request.LastName;
		}

		return Result<UpdateResponse>.Succeed(new UpdateResponse("User updated successfully."));
	}
}