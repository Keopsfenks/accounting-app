using System.Security.Claims;
using Application.Features.Commands.Authentication;
using Application.Services.Authentication;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Commands.Authentications.ChangeCompany;

internal sealed record ChangeCompanyHandler(
	ICompanyRepository     companyRepository,
	ICompanyUserRepository companyUserRepository,
	IHttpContextAccessor   httpContextAccessor,
	IJwtProvider           JwtProvider,
	UserManager<AppUser>   userManager) : IRequestHandler<ChangeCompanyRequest, Result<LoginResponse>>
{
	public async Task<Result<LoginResponse>> Handle(ChangeCompanyRequest request, CancellationToken cancellationToken)
	{
		if (httpContextAccessor.HttpContext is null)
			return (500,"You are not authorized to do this");

		string? userId = httpContextAccessor.HttpContext!.User.FindFirstValue("Id");

		if (string.IsNullOrEmpty(userId))
			return (500,"User not found");

		AppUser? user = await userManager.FindByIdAsync(userId);
		if (user is null)
			return (500,"User not found");

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
														  UserRoles = c
																	 .UserRoles.Where(ur => ur.UserId == user.Id)
																	 .ToList()
													  })
							 .ToListAsync(cancellationToken);

		Guid? companyId = request.CompanyId ?? companies.FirstOrDefault()?.Id;

		var response = await JwtProvider.GenerateJwtToken(user, companyId , companies);

		return response;
	}
}