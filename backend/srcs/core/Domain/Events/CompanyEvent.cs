#nullable enable
	using System.Security.Claims;
	using Domain.Entities;
using Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Events;

public sealed class CompanyEvent(Guid company, string type) : INotification {
	public string  type    { get; private set; } = type;
	public Guid companyId { get; private set; } = company;
}

public sealed class CompanyEventHandler(
	ICompanyUserRepository companyUserRepository,
	ICompanyRepository     companyRepository,
	IUnitOfWork            unitOfWork,
	IHttpContextAccessor   httpContextAccessor,
	UserManager<AppUser>   userManager,
	RoleManager<AppRole>   roleManager) : INotificationHandler<CompanyEvent> {
	public async Task Handle(CompanyEvent notification, CancellationToken cancellationToken) {

		if (httpContextAccessor.HttpContext is null)
			throw new ArgumentNullException();

		string? userId    = httpContextAccessor.HttpContext.User.FindFirstValue("Id");

		if (string.IsNullOrEmpty(userId))
			throw new ArgumentNullException();


		Company? company = await companyRepository.FirstOrDefaultAsync(c => c.Id == notification.companyId, cancellationToken);
		AppUser? user = await userManager.FindByIdAsync(userId);
		AppRole? role = await roleManager.FindByNameAsync("Admin");

		if (user is null)
			throw new ArgumentNullException(nameof(user));
		if (company is null)
			throw new ArgumentNullException(nameof(company));
		if (role is null)
			throw new ArgumentNullException(nameof(role));


		if (notification.type == "create") {
			await CreateCompanyUser(user, company, role, cancellationToken);
		}

		await unitOfWork.SaveChangesAsync(cancellationToken);
	}

	private async Task CreateCompanyUser(
		AppUser           user,
		Company           company,
		AppRole           role,
		CancellationToken cancellationToken) {
		CompanyUsers companyUser = new() {
											 UserId    = user.Id,
											 User      = user,
											 CompanyId = company.Id,
											 Company   = company,
											 RoleName  = role.Name,
											 RoleId    = role.Id
										 };
		await companyUserRepository.AddAsync(companyUser, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}