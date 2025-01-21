using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.CashRegisterDetails.CashRegisterDetailDelete;

internal sealed record CashRegisterDetailDeleteHandler(
	ICashRegisterRepository cashRegisterRepository,
	ICashRegisterDetailRepository cashRegisterDetailRepository,
	IBankDetailRepository bankDetailRepository,
	IBankRepository bankRepository,
	IUnitOfWorkCompany unitOfWorkCompany) : IRequestHandler<CashRegisterDetailDeleteRequest, Result<string>> {
	public async Task<Result<string>> Handle(CashRegisterDetailDeleteRequest request, CancellationToken cancellationToken) {
		CashRegisterDetail? cashRegisterDetail =
			await cashRegisterDetailRepository
			   .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

		if (cashRegisterDetail is null) {
			return Result<string>.Failure("Cash register detail not found");
		}

		CashRegister? cashRegister = await cashRegisterRepository
		   .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.CashRegisterId, cancellationToken);

		if (cashRegister is null) {
			return Result<string>.Failure("Cash register not found");
		}

		cashRegister.DepositAmount    -= cashRegisterDetail.DepositAmount;
		cashRegister.WithdrawalAmount -= cashRegisterDetail.WithdrawalAmount;
		cashRegister.BalanceAmount    -= cashRegisterDetail.DepositAmount - cashRegisterDetail.WithdrawalAmount;

		if (cashRegisterDetail.Opposite["CashRegister"] is not null) {
			CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository
			   .GetByExpressionWithTrackingAsync(x =>
													 x.CashRegisterId == cashRegisterDetail.Opposite["CashRegister"] &&
													 x.Opposite["CashRegister"] == cashRegisterDetail.CashRegisterId,
												 cancellationToken);

			if (oppositeCashRegisterDetail is null)
				return Result<string>.Failure("Opposite cash register detail not found");

			CashRegister? oppositeCashRegister = await cashRegisterRepository
			   .GetByExpressionWithTrackingAsync(x => x.Id == oppositeCashRegisterDetail.CashRegisterId, cancellationToken);

			if (oppositeCashRegister is null)
				return Result<string>.Failure("Opposite cash register not found");

			oppositeCashRegister.DepositAmount -= oppositeCashRegisterDetail.DepositAmount;
			oppositeCashRegister.WithdrawalAmount -= oppositeCashRegisterDetail.WithdrawalAmount;
			oppositeCashRegister.BalanceAmount -= (oppositeCashRegisterDetail.DepositAmount - oppositeCashRegisterDetail.WithdrawalAmount);

			cashRegisterDetailRepository.Delete(oppositeCashRegisterDetail);
		}

		if (cashRegisterDetail.Opposite["Bank"] is not null) {
			BankDetail? oppositeBankDetail = await bankDetailRepository
			   .GetByExpressionWithTrackingAsync(x =>
													 x.BankId         == cashRegisterDetail.Opposite["Bank"],
												 cancellationToken);

			if (oppositeBankDetail is null)
				return (500, "Opposite bank detail not found");

			Bank? oppositeBank = await bankRepository.GetByExpressionWithTrackingAsync(p => p.Id == oppositeBankDetail.BankId, cancellationToken);

			if (oppositeBank is null)
				return (500, "Opposite bank not found");

			oppositeBank.DepositAmount  -= oppositeBankDetail.DepositAmount;
			oppositeBank.WithdrawAmount -= oppositeBankDetail.WithdrawalAmount;
			oppositeBank.Balance        -= (oppositeBankDetail.DepositAmount - oppositeBankDetail.WithdrawalAmount);

			bankDetailRepository.Delete(oppositeBankDetail);
		}
		cashRegisterDetailRepository.Delete(cashRegisterDetail);

		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
		return "Cash register detail deleted successfully";
	}
}