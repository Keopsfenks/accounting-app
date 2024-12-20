using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Queries.Users;

public sealed record GetAllUsers() : IRequest<List<AppUser>> {
	public int PageNumber { get; set; } = 1;
	public int PageSize   { get; set; } = 10;
}
internal sealed class GetAllUsersHandler : IRequestHandler<GetAllUsers, List<AppUser>>
{
	private readonly UserManager<AppUser> _userManager;

	public GetAllUsersHandler(UserManager<AppUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<List<AppUser>> Handle(GetAllUsers request, CancellationToken cancellationToken)
	{
		// Sayfa numarasını ve sayfa başına gösterilecek öğe sayısını alıyoruz
		var pageNumber = request.PageNumber;
		var pageSize   = request.PageSize;

		// Veritabanından kullanıcıları getiriyoruz
		List<AppUser> users = await _userManager.Users
		                                        .OrderBy(p => p.FirstName)
		                                        .Skip((pageNumber - 1) * pageSize)  // Skip: Önceki sayfalardaki öğeleri atlamak
		                                        .Take(pageSize)                      // Take: Bu sayfada alacağımız öğe sayısını belirlemek
		                                        .ToListAsync(cancellationToken);

		return users;
	}
}