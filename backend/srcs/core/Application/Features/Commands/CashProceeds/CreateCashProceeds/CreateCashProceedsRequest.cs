using Domain.Entities.CompanyEntities;
using Domain.Enums;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.CashProceeds.CreateCashProceeds;

public sealed record CreateCashProceedsRequest(
	string Description,
	string IssueDate,
	decimal Amount,
	int Payment,
	int Operation,
	Cheque? Cheque,
	Guid? InvoiceId,
	Guid? CustomerDetailId
	) : IRequest<Result<string>>;




internal sealed record CreateCahProceedsHandler(
	IInvoiceRepository        invoiceRepository,
	ICustomerRepository       customerRepository,
	ICustomerDetailRepository customerDetailRepository,
	ICashProceedRepository    cashProceedRepository,
	IUnitOfWorkCompany        unitOfWorkCompany) : IRequestHandler<CreateCashProceedsRequest, Result<string>> {
	public async Task<Result<string>> Handle(CreateCashProceedsRequest request, CancellationToken cancellationToken) {

		CashProceed cashProceed = new CashProceed {
													  Description = request.Description,
													  IssueDate   = request.IssueDate,
													  Amount      = request.Amount,
													  PaymentType = PaymentTypeEnum.FromValue(request.Payment),
													  Operation   = OperationTypeEnum.FromValue(request.Operation),
													  Cheque      = request.Cheque,
													  InvoiceId   = request.InvoiceId,
													  CustomerDetailId = request.CustomerDetailId,
													  Invoice = null,
													  CustomerDetail = null
												  };



		if (cashProceed.InvoiceId is not null) {
			Invoice? invoice = await invoiceRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashProceed.InvoiceId, cancellationToken);

			if (invoice is null)
				return (500, "Invoice not found");

			Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == invoice.CustomerId, cancellationToken);

			if (customer is null)
				return (500, "Customer not found");

			cashProceed.Invoice = invoice;

			if ((invoice.TotalAmount + request.Amount) > 0)
				return (500, "Amount is greater than the invoice amount");

			invoice.DepositAmount += cashProceed.Amount;
			invoice.TotalAmount += cashProceed.Amount;

			customer.Deposit += request.Amount;
			customer.Debit += request.Amount;

			if (invoice.TotalAmount == 0)
				invoice.Status = StatusEnum.Paid;

			customerRepository.Update(customer);
			invoiceRepository.Update(invoice);
		} else if (cashProceed.CustomerDetailId is not null) {
			CustomerDetail? customerDetail = await customerDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashProceed.CustomerDetailId, cancellationToken);

			if (customerDetail is null)
				return (500, "Sales transaction is  not found");

			Customer? customer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == customerDetail.CustomerId, cancellationToken);

			if (customer is null)
				return (500, "Customer not found");

			cashProceed.CustomerDetail = customerDetail;

			if ((customerDetail.TotalAmount + request.Amount) > 0)
				return (500, "Amount is greater than the sales transaction amount");
			customerDetail.DepositAmount += cashProceed.Amount;
			customerDetail.TotalAmount += cashProceed.Amount;

			customer.Deposit += request.Amount;
			customer.Debit   += request.Amount;

			if (customerDetail.TotalAmount == 0)
				customerDetail.Status = StatusEnum.Paid;

			customerRepository.Update(customer);
			customerDetailRepository.Update(customerDetail);
		}

		await cashProceedRepository.AddAsync(cashProceed, cancellationToken);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return ("Cash Proceeds created successfully");
	}
}