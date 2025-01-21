using System.Globalization;
using System.Security.Claims;
using Domain.Entities.CompanyEntities;
using Domain.Entities.CompanyEntities.Products;
using Domain.Enums;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Invoices.CreateInvoice;

public sealed record InvoiceProduct(
	Guid   ProductId,
	Pricing Pricing);


public record CreateInvoiceRequest(
	string               InvoiceNumber,
	string               Description,
	DateOnly             IssueDate,
	DateOnly?            DueDate,
	Guid?                Company,
	Guid                 CustomerId,
	List<InvoiceProduct> Products,
	int                  CurrencyType,
	int                  Payment,
	int                  Operation,
	int                  Status
) : IRequest<Result<string>>;


internal sealed record CreateInvoiceHandler(
	IHttpContextAccessor     httpContextAccessor,
	IProductDetailRepository productDetailRepository,
	IProductRepository       productRepository,
	IUnitOfWorkCompany       unitOfWorkCompany,
	ICustomerRepository      customerRepository,
	IInvoiceRepository       invoiceRepository) : IRequestHandler<CreateInvoiceRequest, Result<string>> {
	public async Task<Result<string>> Handle(CreateInvoiceRequest request, CancellationToken cancellationToken) {
		decimal depositAmount = 0;
		decimal withdrawalAmount = 0;
		if (httpContextAccessor.HttpContext is null)
			return (500, "You are not authorized to do this");

		string? userId   = httpContextAccessor.HttpContext.User.FindFirstValue("Id");
		string? userName = httpContextAccessor.HttpContext.User.FindFirstValue("Username");

		if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
			return (500, "User not found");
		bool isExist = await invoiceRepository.AnyAsync(c => c.InvoiceNumber == request.InvoiceNumber, cancellationToken);

		if (isExist)
			return (500, "Invoice is already exist!");

		Invoice invoice = new() {
									Processor = new Dictionary<string, Guid> {
																				 { userName, Guid.Parse(userId) }
																			 },
									Description  = request.Description,
									CompanyId    = request.Company,
									CurrencyType = CurrencyTypeEnum.FromValue(request.CurrencyType),
									Status       = StatusEnum.FromValue(request.Status),
									Operation    = OperationTypeEnum.FromValue(request.Operation),
									Payment      = PaymentEnum.FromValue(request.Payment),
									IssueDate = request.IssueDate.ToString("dd MMMM yyyy", new CultureInfo("tr-TR")),
									DueDate = request.DueDate?.ToString("dd MMMM yyyy", new CultureInfo("tr-TR")),
									CustomerId = request.CustomerId,
									InvoiceNumber = request.InvoiceNumber,
								};

		foreach (InvoiceProduct product in request.Products) {
			Product? productEntity = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == product.ProductId, cancellationToken);

			if (productEntity is null)
				return (500, "Product is not found!");

			ProductDetail productDetail = new() {
													Processor   = invoice.Processor,
													Description = "Invoiced sale named " + request.InvoiceNumber,
													Date = request.IssueDate.ToString(
														"dd MMMM yyyy", new CultureInfo("tr-TR")),
													Invoice          = invoice,
													InvoiceId        = invoice.Id,
													ProductId        = productEntity.Id,
													Product          = productEntity,
													Pricing          = product.Pricing,
													Type             = OperationTypeEnum.FromValue(request.Operation),
													CustomerDetail   = null,
													CustomerDetailId = null,
												};
			depositAmount += productDetail.Type == OperationTypeEnum.Sales ? productDetail.Pricing.TotalPrice : 0;
			withdrawalAmount += productDetail.Type == OperationTypeEnum.CashProceeds ? productDetail.Pricing.TotalPrice : 0;

			await productDetailRepository.AddAsync(productDetail, cancellationToken);
		}

		Customer? customer
			= await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.CustomerId,
																		cancellationToken);

		if (customer is null)
			return (500, "Customer is not found");

		invoice.Customer = customer;
		switch (request.Status) {
			case 1:
				break;
			case 2:
				invoice.DepositAmount    = withdrawalAmount;
				invoice.WithdrawalAmount = depositAmount;
				invoice.TotalAmount      = 0 - depositAmount;
				break;
			case 3:
				invoice.DepositAmount    = depositAmount;
				invoice.WithdrawalAmount = depositAmount;
				invoice.TotalAmount      = 0;
				break;
			case 4:
				break;
		}

		customer.Deposit    += invoice.DepositAmount;
		customer.Withdrawal += invoice.WithdrawalAmount;
		customer.Debit      += invoice.TotalAmount;

		customerRepository.Update(customer);
		await invoiceRepository.AddAsync(invoice, cancellationToken);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
		return "Invoice Created Successfully";
	}
}