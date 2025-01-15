using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.BankDetails.BankDetailDelete;

internal sealed record BankDetailDeleteHandler(
	IBankRepository               bankRepository,
	IBankDetailRepository         bankDetailRepository,
	ICashRegisterDetailRepository cashRegisterDetailRepository,
	ICashRegisterRepository       cashRegisterRepository,
	IUnitOfWorkCompany            unitOfWorkCompany) : IRequestHandler<BankDetailDeleteRequest, Result<string>> {
	public async Task<Result<string>> Handle(BankDetailDeleteRequest request, CancellationToken cancellationToken) {
		BankDetail? bankDetail = await bankDetailRepository
		   .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

		if (bankDetail is null)
			return (500, "Bank detail not found");

		Bank? bank = await bankRepository
		   .GetByExpressionWithTrackingAsync(p => p.Id == bankDetail.BankId, cancellationToken);

		if (bank is null)
			return (500, "Bank not found");

		bank.DepositAmount  -= bankDetail.DepositAmount;
		bank.WithdrawAmount -= bankDetail.WithdrawalAmount;
		bank.Balance        -= bankDetail.DepositAmount - bankDetail.WithdrawalAmount;

		if (bankDetail.Opposite["Bank"] is not null) {
			BankDetail? oppositeBankDetail = await bankDetailRepository
			   .GetByExpressionWithTrackingAsync(x =>
													 x.BankId           == bankDetail.Opposite["Bank"] &&
													 x.Opposite["Bank"] == bankDetail.BankId,
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

		if (bankDetail.Opposite["CashRegister"] is not null) {
			CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository
			   .GetByExpressionWithTrackingAsync(x =>
													 x.CashRegisterId == bankDetail.Opposite["CashRegister"],
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

		bankDetailRepository.Delete(bankDetail);

		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
		return ("Bank detail deleted successfully");
	}
}