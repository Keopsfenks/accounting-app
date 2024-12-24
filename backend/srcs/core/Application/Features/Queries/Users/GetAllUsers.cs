using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Users;

public sealed record GetAllUsers() : IRequest<Result<List<AppUser>>> {
	public int PageNumber { get; set; } = 0;
	public int PageSize   { get; set; } = 10;

	public string? Id { get; set; }
}
internal sealed record GetAllUsersHandler(
	UserManager<AppUser> userManager) : IRequestHandler<GetAllUsers, Result<List<AppUser>>>
{

	public async Task<Result<List<AppUser>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
	{
		int     pageNumber = request.PageNumber;
		int     pageSize   = request.PageSize;
		string? Id         = request.Id;


		List<AppUser> users;
		if (Id is not null) {
			users = await userManager.Users
									 .Where(p => p.Id.ToString() == Id)
									 .OrderBy(p => p.Id)
									 .Include(p => p.UserRoles)
									 .Skip(pageNumber * pageSize)
									 .Take(pageSize)
									 .ToListAsync(cancellationToken);
			return users;
		}

		users = await userManager.Users
								 .OrderBy(p => p.FirstName)
								 .Skip((pageNumber) * pageSize)
								 .Include(p => p.UserRoles)
								 .Take(pageSize)
								 .ToListAsync(cancellationToken);

		return users;
	}
}