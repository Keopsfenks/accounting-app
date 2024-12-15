using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Queries.Users;

public sealed record GetAllUsers() : IRequest<List<AppUser>> { }

internal sealed class GetAllUsersHandler(
	UserManager<AppUser> userManager) : IRequestHandler<GetAllUsers, List<AppUser>> {
	public async Task<List<AppUser>> Handle(GetAllUsers request, CancellationToken cancellationToken) {
		List<AppUser> users = await userManager.Users.OrderBy(p => p.FirstName).ToListAsync(cancellationToken);
		return users; 
	}
}