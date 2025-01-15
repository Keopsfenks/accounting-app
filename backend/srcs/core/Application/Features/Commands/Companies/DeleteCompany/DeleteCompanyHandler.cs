using Application.Services.Companies;
using Domain.Entities;
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
	ICompanyService companyService,
	IMediator mediator) : IRequestHandler<DeleteCompanyRequest, Result<string>> {
	public async Task<Result<string>> Handle(DeleteCompanyRequest request, CancellationToken cancellationToken) {
		if (request.companyId == Guid.Empty)
			return Result<string>.Failure("Şirket kimliği boş olamaz");

		Company company = await companyRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.companyId, cancellationToken);
		if (company is null)
			return Result<string>.Failure("Şirket bulunamadı");

		companyService.DeleteCompanyDatabase(company);
		companyRepository.Delete(company);
		await unitOfWork.SaveChangesAsync(cancellationToken);


		return "Şirket başarıyla silindi";
	}
}