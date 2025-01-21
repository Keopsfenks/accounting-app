using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.CustomerDetails.DeleteCustomerDetails;

public sealed record DeleteCustomerDetailRequest(
	Guid Id) : IRequest<Result<string>>;


internal sealed record DeleteCustomerDetailHandler(
	IProductDetailRepository      productDetailRepository,
	IProductRepository            productRepository,
	ICashRegisterDetailRepository cashRegisterDetailRepository,
	ICashRegisterRepository       cashRegisterRepository,
	IUnitOfWorkCompany            unitOfWorkCompany,
	ICustomerRepository           customerRepository,
	ICustomerDetailRepository     customerDetailRepository,
	IInvoiceRepository            invoiceRepository) : IRequestHandler<DeleteCustomerDetailRequest, Result<string>> {
	public async Task<Result<string>> Handle(DeleteCustomerDetailRequest request, CancellationToken cancellationToken) {
		CustomerDetail? customerDetail = await customerDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);

		if (customerDetail is null)
			return (404, "Customer detail not found");

		Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == customerDetail.CustomerId, cancellationToken);

		if (customer is null)
			return (404, "Customer not found");

		CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == customerDetail.CashRegisterDetailId, cancellationToken);

		if (cashRegisterDetail is null)
			return (404, "Cash register detail not found");

		CashRegister? cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashRegisterDetail.CashRegisterId, cancellationToken);

		if (cashRegister is null)
			return (404, "Cash register not found");

		customer.Deposit -= (
			(customerDetail.DepositAmount) +
			(customerDetail.CashProceeds != null && customerDetail.CashProceeds.Count > 0
				? customerDetail.CashProceeds.Sum(c => c.Amount)
				: 0)
		);		customer.Withdrawal -= customerDetail.WithdrawalAmount;
		customer.Debit                -= customerDetail.TotalAmount;

		cashRegister.DepositAmount -= cashRegisterDetail.DepositAmount;
		cashRegister.WithdrawalAmount -= cashRegisterDetail.WithdrawalAmount;
		cashRegister.BalanceAmount -= cashRegisterDetail.DepositAmount - cashRegisterDetail.WithdrawalAmount;

		cashRegisterRepository.Update(cashRegister);
		cashRegisterDetailRepository.Delete(cashRegisterDetail);

		customerRepository.Update(customer);
		customerDetailRepository.Delete(customerDetail);

		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
		return ("Sales transaction is deleted successfully");
	}
}
