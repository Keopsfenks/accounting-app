using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.CashProceeds.DeleteCashProceeds;

public sealed record DeleteCashProceedsRequest(
	Guid Id) : IRequest<Result<string>>;


internal sealed record DeleteCashProceedsHandler(
	IInvoiceRepository        invoiceRepository,
	ICustomerRepository       customerRepository,
	ICustomerDetailRepository customerDetailRepository,
	ICashProceedRepository    cashProceedRepository,
	IUnitOfWorkCompany        unitOfWorkCompany) : IRequestHandler<DeleteCashProceedsRequest, Result<string>> {
	public async Task<Result<string>> Handle(DeleteCashProceedsRequest request, CancellationToken cancellationToken) {
		CashProceed? cashProceed = await cashProceedRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);

		if (cashProceed is null)
			return (500, "Cash Proceeds not found");

		if (cashProceed.InvoiceId is not null) {
			Invoice? invoice = await invoiceRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashProceed.InvoiceId, cancellationToken);

			if (invoice is null)
				return (500, "Invoice not found");

			Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == invoice.CustomerId, cancellationToken);

			if (customer is null)
				return (500, "Customer not found");

			invoice.DepositAmount -= cashProceed.Amount;
			invoice.TotalAmount -= cashProceed.Amount;

			customer.Deposit -= cashProceed.Amount;
			customer.Debit -= cashProceed.Amount;

			customerRepository.Update(customer);
			invoiceRepository.Update(invoice);
		} else if (cashProceed.CustomerDetailId is not null) {
			CustomerDetail? customerDetail = await customerDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashProceed.CustomerDetailId, cancellationToken);

			if (customerDetail is null)
				return (500, "Customer Detail not found");

			Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == customerDetail.CustomerId, cancellationToken);

			if (customer is null)
				return (500, "Customer not found");

			customerDetail.DepositAmount -= cashProceed.Amount;
			customerDetail.TotalAmount -= cashProceed.Amount;

			customer.Deposit -= cashProceed.Amount;
			customer.Debit   -= cashProceed.Amount;

			customerRepository.Update(customer);
			customerDetailRepository.Update(customerDetail);
		}


		cashProceedRepository.Delete(cashProceed);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return ("Cash Proceeds deleted successfully");
	}
}