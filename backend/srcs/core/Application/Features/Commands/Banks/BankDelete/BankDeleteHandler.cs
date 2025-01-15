using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Banks.BankDelete;

internal sealed record BankDeleteHandler(
	IBankRepository    bankRepository,
	IUnitOfWorkCompany unitOfWorkCompany) : IRequestHandler<BankDeleteRequest, Result<string>> {
	public async Task<Result<string>> Handle(BankDeleteRequest request, CancellationToken cancellationToken) {
		Bank? bank = await bankRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

		if (bank is null)
			return (500, "Bank not found");

		bankRepository.Delete(bank);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
		
		return ("Bank deleted successfully");
	}
}