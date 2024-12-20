using AutoMapper;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Commands.Users.CreateUser;

internal sealed record CreateHandler(
	UserManager<AppUser> userManager,
	IMapper mapper,
	IMediator mediator) : IRequestHandler<RegisterRequest, Result<string>> {
	public async Task<Result<string>> Handle(RegisterRequest request, CancellationToken cancellationToken) {
		bool userExists = await userManager.Users.AnyAsync(p => p.UserName == request.UserName, cancellationToken);
		
		if (userExists) {
			return Result<string>.Failure("User already exists");
		}
		
		bool emailExists = await userManager.Users.AnyAsync(p => p.Email == request.Email, cancellationToken);
		
		if (emailExists) {
			return Result<string>.Failure("Email already exists");
		}

		AppUser newUser = mapper.Map<AppUser>(request);
		
		IdentityResult result = await userManager.CreateAsync(newUser, request.Password);
		
		if (!result.Succeeded) {
			return Result<string>.Failure(result.Errors.Select(s => s.Description).ToList());
		}
		
		//Email Confirmation mail sending logic
		await mediator.Publish(new UserEvents(newUser.Id), cancellationToken);
		
		return Result<string>.Succeed("User created successfully");
	}
}