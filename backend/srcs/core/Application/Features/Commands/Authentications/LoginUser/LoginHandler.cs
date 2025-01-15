using Application.Services.Authentication;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Commands.Authentication;

internal sealed record LoginHandler(
	UserManager<AppUser> userManager,
	IJwtProvider  JwtProvider,
	ICompanyUserRepository companyUserRepository,
	ICompanyRepository companyRepository
	) : IRequestHandler<LoginRequest, Result<LoginResponse>> {
	
	public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken) {
		AppUser? user = await userManager.Users.FirstOrDefaultAsync(
			p => p.UserName == request.EmailOrUsername || p.Email == request.EmailOrUsername, cancellationToken);

		if (user == null) {
			return (500, "User not found");
		}

		if (await userManager.IsLockedOutAsync(user)) {
			return (500, "User account is locked out.");
		}

		var isPasswordCheck = await userManager.CheckPasswordAsync(user, request.Password);

		if (!isPasswordCheck) {
			await userManager.AccessFailedAsync(user);

			var failedAccessCount = await userManager.GetAccessFailedCountAsync(user);

			if (failedAccessCount >= 3) {
				await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(5));
				return (500,"User is locked out for five minutes");
			}

			return (500,"Invalid password");
		}

		if (!user.EmailConfirmed) {
			return (500,"User email is not confirmed");
		}

		var companyUsers = await companyUserRepository
								.Where(p => p.UserId == user.Id)
								.ToListAsync(cancellationToken);

		if (!companyUsers.Any())
			return await JwtProvider.GenerateJwtToken(user, null, new List<Company>());

		var companyIds = companyUsers.Select(cu => cu.CompanyId).ToList();
		var companies = await companyRepository
							 .Where(c => companyIds.Contains(c.Id))
							 .Select(c => new Company {
														  Id            = c.Id,
														  Name          = c.Name,
														  TaxId         = c.TaxId,
														  TaxDepartment = c.TaxDepartment,
														  Address       = c.Address,
														  Database      = c.Database,
														  CreatedAt     = c.CreatedAt,
														  UpdatedAt     = c.UpdatedAt,
														  UserRoles = c
																	 .UserRoles.Where(ur => ur.UserId == user.Id)
																	 .ToList()
													  })
							 .ToListAsync(cancellationToken);

		var loginResponse = await JwtProvider.GenerateJwtToken(user, companies.First().Id, companies);

		return loginResponse;
	}
}