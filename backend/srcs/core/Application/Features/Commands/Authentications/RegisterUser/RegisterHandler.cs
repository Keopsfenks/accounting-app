using Application.Features.Commands.Users.RegisterUser;
using AutoMapper;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Commands.Users.CreateUser;

internal sealed record RegisterHandler(
	UserManager<AppUser> userManager,
	IMapper mapper,
	IMediator mediator) : IRequestHandler<RegisterRequest, Result<string>> {
	public async Task<Result<string>> Handle(RegisterRequest request, CancellationToken cancellationToken) {
		bool userExists = await userManager.Users.AnyAsync(p => p.UserName == request.UserName, cancellationToken);
		
		if (userExists) {
			return (500, "User already exists");
		}
		
		bool emailExists = await userManager.Users.AnyAsync(p => p.Email == request.Email, cancellationToken);
		
		if (emailExists) {
			return (500, "Email already exists");
		}

		AppUser newUser = mapper.Map<AppUser>(request);

		IdentityResult result = await userManager.CreateAsync(newUser, request.Password);
		
		if (!result.Succeeded) {
			return (500, result.Errors.Select(s => s.Description).ToList());
		}

		try {
			await mediator.Publish(new UserEvents(newUser.Id),       cancellationToken);
		}
		catch (Exception e) {
			return (500, e.Message);
		}


		return Result<string>.Succeed("User created successfully");
	}
}