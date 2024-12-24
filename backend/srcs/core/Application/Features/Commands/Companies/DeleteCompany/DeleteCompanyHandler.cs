using Domain.Entities;
using Domain.Events;
using Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace Application.Features.Commands.Companies.DeleteCompany;

public sealed record DeleteCompanyHandler(
	ICompanyRepository companyRepository,
	IUnitOfWork        unitOfWork,
	UserManager<AppUser> userManager,
	IMediator mediator) : IRequestHandler<DeleteCompanyRequest, Result<string>> {
	public async Task<Result<string>> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken) {
		Company company = await companyRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.companyId, cancellationToken);
		AppUser? user = await userManager.FindByIdAsync(request.UserId.ToString());

		if (user == null) {
			return Result<string>.Failure("User not found");
		}
		if (company is null)
		{
			return Result<string>.Failure("Şirket bulunamadı");
		}

		companyRepository.Delete(company);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return "Şirket başarıyla silindi";
	}
}