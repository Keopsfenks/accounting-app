using System.Security.Claims;
using Application.Services.Companies;
using AutoMapper;
using Domain.Entities;
using Domain.Events;
using Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Commands.Companies.CreateCompany;

public sealed record CreateCompanyHandler(
	ICompanyRepository     companyRepository,
	ICompanyUserRepository companyUserRepository,
	IHttpContextAccessor   httpContextAccessor,
	IUnitOfWork            unitOfWork,
	IMapper                mapper,
	IMediator              mediator,
	UserManager<AppUser>  userManager,
	RoleManager<AppRole>  roleManager,
	ICompanyService        companyService) : IRequestHandler<CreateCompanyRequest, Result<string>> {
	public async Task<Result<string>> Handle(CreateCompanyRequest request, CancellationToken cancellationToken) {
		if (httpContextAccessor.HttpContext is null)
			return (500, "Internal Server Error HttpContext is null");

		string? userId = httpContextAccessor.HttpContext.User.FindFirstValue("Id");

		if (string.IsNullOrEmpty(userId))
			return (500, "Internal Server Error User Id is null");

		AppUser? user = await userManager.FindByIdAsync(userId);
		AppRole? role = await roleManager.FindByNameAsync("Admin");

		if (user is null)
			return (500, "Internal Server Error User is null");

		if (role is null)
			return (500, "Internal Server Error Role is null");

		bool isTaxNumberExists = await companyRepository.AnyAsync(p=> p.TaxId  == request.TaxId, cancellationToken);

		if (isTaxNumberExists) {
			return Result<string>.Failure("Tax number already exists");
		}

		Company company = mapper.Map<Company>(request);
		CompanyUsers companyUser = new() {
											 UserId    = user.Id,
											 User      = user,
											 CompanyId = company.Id,
											 Company   = company,
											 RoleId    = role.Id,
											 RoleName  = role.Name,
										 };


		await companyRepository.AddAsync(company, cancellationToken);
		await companyUserRepository.AddAsync(companyUser, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);

		companyService.MigrateCompanyDatabase(company);

		return "Company created successfully";
	}
}