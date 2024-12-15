using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Commands.Users.CreateUser;

internal sealed record CreateHandler(
	UserManager<AppUser> userManager,
	IMapper mapper) : IRequestHandler<RegisterRequest, Result<RegisterResponse>> {
	public async Task<Result<RegisterResponse>> Handle(RegisterRequest request, CancellationToken cancellationToken) {
		bool userExists = await userManager.Users.AnyAsync(p => p.UserName == request.UserName, cancellationToken);
		
		if (userExists) {
			return Result<RegisterResponse>.Failure("User already exists");
		}
		
		bool emailExists = await userManager.Users.AnyAsync(p => p.Email == request.Email, cancellationToken);
		
		if (emailExists) {
			return Result<RegisterResponse>.Failure("Email already exists");
		}

		AppUser newUser = mapper.Map<AppUser>(request);
		
		IdentityResult result = await userManager.CreateAsync(newUser, request.Password);
		
		if (!result.Succeeded) {
			return Result<RegisterResponse>.Failure(result.Errors.Select(s => s.Description).ToList());
		}
		
		//Email Confirmation mail sending logic
		
		return Result<RegisterResponse>.Succeed(new RegisterResponse("User created successfully"));
	}
}