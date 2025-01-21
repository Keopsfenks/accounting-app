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

namespace Application.Features.Commands.CustomerDetails.CreateCustomeDetails;


public sealed record SalesProduct(
	Guid    ProductId,
	Pricing Pricing);

public sealed record CreateCustomerDetailRequest(
	string                Name,
	string                Description,
	DateOnly              IssueDate,
	DateOnly?             DueDate,
	decimal               Amount,
	int                   Payment,
	int                   Operation,
	int                   Status,
	Guid                  cashRegisterId,
	Guid                  CustomerId,
	List<SalesProduct>? Products) : IRequest<Result<string>>;




internal sealed record CreateCustomerDetailHanddler(
	IHttpContextAccessor          httpContextAccessor,
	IProductDetailRepository      productDetailRepository,
	IProductRepository            productRepository,
	IUnitOfWorkCompany            unitOfWorkCompany,
	ICustomerRepository           customerRepository,
	ICashRegisterRepository       cashRegisterRepository,
	ICashRegisterDetailRepository cashRegisterDetailRepository,
	ICustomerDetailRepository     customerDetailRepository) : IRequestHandler<CreateCustomerDetailRequest, Result<string>> {
	public async Task<Result<string>> Handle(CreateCustomerDetailRequest request, CancellationToken cancellationToken) {
		decimal depositAmount    = 0;
		decimal withdrawalAmount = 0;
		if (httpContextAccessor.HttpContext is null)
			return (500, "You are not authorized to do this");

		string? userId   = httpContextAccessor.HttpContext.User.FindFirstValue("Id");
		string? userName = httpContextAccessor.HttpContext.User.FindFirstValue("Username");

		if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
			return (500, "User not found");

		CustomerDetail customerDetail = new() {
												  Processor = new Dictionary<string, Guid> {
																  { userName, Guid.Parse(userId) }
															  },
												  Name        = request.Name,
												  Description = request.Description,
												  IssueDate = request.IssueDate.ToString(
													  "dd MMMM yyyy", new CultureInfo("tr-TR")),
												  DueDate = request.DueDate?.ToString(
													  "dd MMMM yyyy", new CultureInfo("tr-TR")),
												  CustomerId = request.CustomerId,
												  Status     = StatusEnum.FromValue(request.Status),
												  Operation  = OperationTypeEnum.FromValue(request.Operation),
												  Payment    = PaymentEnum.FromValue(request.Payment),
											  };

		if (request.Products is not null) {
			foreach (SalesProduct product in request.Products) {
				Product? productEntity = await productRepository.GetByExpressionWithTrackingAsync(p => p.Id == product.ProductId, cancellationToken);

				if (productEntity is null)
					return (500, "Product is not found!");

				ProductDetail productDetail = new() {
														Processor   = customerDetail.Processor,
														Description = "Sales transaction named " + request.Name,
														Date = request.IssueDate.ToString(
															"dd MMMM yyyy", new CultureInfo("tr-TR")),
														Invoice          = null,
														InvoiceId        = null,
														ProductId        = productEntity.Id,
														Product          = productEntity,
														Pricing          = product.Pricing,
														Type             = OperationTypeEnum.FromValue(request.Operation),
														CustomerDetail   = customerDetail,
														CustomerDetailId = customerDetail.Id,
													};
				depositAmount += productDetail.Type == OperationTypeEnum.Sales ? productDetail.Pricing.TotalPrice : 0;
				withdrawalAmount += productDetail.Type == OperationTypeEnum.CashProceeds ? productDetail.Pricing.TotalPrice : 0;

				await productDetailRepository.AddAsync(productDetail, cancellationToken);
			}
		}
		else {
			depositAmount += request.Operation == OperationTypeEnum.Sales ? request.Amount : 0;
			withdrawalAmount += request.Operation == OperationTypeEnum.CashProceeds ? request.Amount : 0;
		}

		Customer? customer
			= await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.CustomerId,
																		cancellationToken);

		if (customer is null)
			return (500, "Customer is not found");

		CashRegister? cashRegister
			= await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.cashRegisterId,
																			cancellationToken);

		if (cashRegister is null)
			return (500, "Cash register is not found");

		CashRegisterDetail cashRegisterDetail = new() {
														  Processor        = customerDetail.Processor,
														  Description      = "Sales transaction named " + request.Name,
														  DepositAmount    = depositAmount,
														  WithdrawalAmount = withdrawalAmount,
														  Date = request.IssueDate.ToString(
															  "dd MMMM yyyy", new CultureInfo("tr-TR")),
														  Opposite = new Dictionary<string, Guid?> {
																			 { "Customer", customerDetail.Id },
																			 { "CashRegister", null },
																			 { "Bank", null },
																		 },
														  CashRegister   = cashRegister,
														  CashRegisterId = cashRegister.Id
													  };

		cashRegister.DepositAmount	+= cashRegisterDetail.DepositAmount;
		cashRegister.WithdrawalAmount += cashRegisterDetail.WithdrawalAmount;
		cashRegister.BalanceAmount	+= cashRegisterDetail.DepositAmount - cashRegisterDetail.WithdrawalAmount;
		await cashRegisterDetailRepository.AddAsync(cashRegisterDetail, cancellationToken);

		customerDetail.Customer = customer;
		customerDetail.CustomerId = customer.Id;
		customerDetail.CashRegisterDetailId = cashRegisterDetail.Id;
		switch (request.Status) {
			case 1:
				break;
			case 2:
				customerDetail.DepositAmount    = withdrawalAmount;
				customerDetail.WithdrawalAmount = depositAmount;
				customerDetail.TotalAmount      = 0 - depositAmount;
				break;
			case 3:
				customerDetail.DepositAmount    = depositAmount;
				customerDetail.WithdrawalAmount = depositAmount;
				customerDetail.TotalAmount      = 0;
				break;
			case 4:
				break;
		}

		customer.Deposit    += customerDetail.DepositAmount;
		customer.Withdrawal += customerDetail.WithdrawalAmount;
		customer.Debit      += customerDetail.TotalAmount;

		cashRegisterRepository.Update(cashRegister);
		customerRepository.Update(customer);
		await customerDetailRepository.AddAsync(customerDetail, cancellationToken);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
		return ("Sales transaction created successfully");
	}
}
