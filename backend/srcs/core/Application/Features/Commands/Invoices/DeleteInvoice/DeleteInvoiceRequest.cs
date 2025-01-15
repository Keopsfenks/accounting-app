using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Invoices.DeleteInvoice;

public sealed record DeleteInvoiceRequest(
	Guid Id) : IRequest<Result<string>>;



internal sealed record DeleteInvoiceHandler(
	IInvoiceRepository invoiceRepository,
	ICustomerRepository customerRepository,
	IUnitOfWorkCompany unitOfWorkCompany) : IRequestHandler<DeleteInvoiceRequest, Result<string>> {
	public async Task<Result<string>> Handle(DeleteInvoiceRequest request, CancellationToken cancellationToken) {
		Invoice? invoice = await invoiceRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

		if (invoice is null)
			return (500, "Invoice is not found!");

		Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(p => p.Id == invoice.CustomerId, cancellationToken);

		if (customer is null)
			return (500, "Customer is not found!");

		customer.Deposit -= invoice.DepositAmount;
		customer.Withdrawal -= invoice.WithdrawalAmount;
		customer.Debit -= invoice.TotalAmount;

		invoiceRepository.Delete(invoice);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return "Invoice Successfully Deleted";
	}
}