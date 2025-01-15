using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.CashRegisters.CashRegisterDelete;

internal sealed record CashRegisterDeleteHandler(
	ICashRegisterRepository cashRegisterRepository,
	IUnitOfWorkCompany unitOfWorkCompany) : IRequestHandler<CashRegisterDeleteRequest, Result<string>> {
	public async Task<Result<string>> Handle(CashRegisterDeleteRequest request, CancellationToken cancellationToken) {
		CashRegister? cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

		if (cashRegister is null) {
			return Result<string>.Failure("Cash register not found");
		}

		cashRegisterRepository.Delete(cashRegister);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return "Cash register deleted successfully";
	}
}