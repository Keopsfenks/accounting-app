using Domain.Entities;
using Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Domain.Events;

public sealed class CompanyEvent : INotification {
	public string type { get; private set; }
	public Guid   UserId { get; private set; }
	public Guid   CompanyId { get; private set; }
	public CompanyEvent(Guid userId, Guid companyId, string type) {
		UserId = userId;
		CompanyId = companyId;
		this.type = type;
	}
}

public sealed class CompanyEventHandler(
	ICompanyUserRepository companyUserRepository,
	ICompanyRepository     companyRepository,
	IUnitOfWork            unitOfWork,
	UserManager<AppUser>   userManager,
	RoleManager<AppRole>   roleManager) : INotificationHandler<CompanyEvent> {
	public async Task Handle(CompanyEvent notification, CancellationToken cancellationToken) {
		AppUser? user = await userManager.FindByIdAsync(notification.UserId.ToString());
		Company? company = await companyRepository.FirstOrDefaultAsync(c => c.Id == notification.CompanyId, cancellationToken);
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
											 Role      = role,
											 RoleId    = role.Id
										 };
		await companyUserRepository.AddAsync(companyUser, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}