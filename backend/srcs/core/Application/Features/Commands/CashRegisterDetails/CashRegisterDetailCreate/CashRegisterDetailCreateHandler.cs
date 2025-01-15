using System.Globalization;
using System.Security.Claims;
using System.Xml.Linq;
using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.CashRegisterDetails.CashRegisterDetailCreate;

internal sealed record CashRegisterDetailCreateHandler(
	ICashRegisterRepository cashRegisterRepository,
	ICashRegisterDetailRepository cashRegisterDetailRepository,
	IBankDetailRepository bankDetailRepository,
	IBankRepository bankRepository,
	IUnitOfWorkCompany unitOfWorkCompany,
	IHttpContextAccessor httpContextAccessor) : IRequestHandler<CashRegisterDetailCreateRequest, Result<string>> {
	public async Task<Result<string>> Handle(CashRegisterDetailCreateRequest request, CancellationToken cancellationToken) {
		if (httpContextAccessor.HttpContext is null)
			return (500, "You are not authorized to do this");

		string? userId = httpContextAccessor.HttpContext.User.FindFirstValue("Id");
		string? userName = httpContextAccessor.HttpContext.User.FindFirstValue("Username");

		if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
			return (500, "User not found");

		CashRegister? cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.CashRegisterId, cancellationToken);

		cashRegister.DepositAmount    += (request.Type == 0 ? request.Amount : 0);
		cashRegister.WithdrawalAmount += (request.Type == 1 ? request.Amount : 0);
		cashRegister.BalanceAmount	+= (request.Type == 0 ? request.Amount : 0) - (request.Type == 1 ? request.Amount : 0);

		CashRegisterDetail cashRegisterDetail = new() {
														  Processor = new Dictionary<string, Guid> {
																		  { userName, Guid.Parse(userId) }
																	  },
														  Date             = request.Date.ToString("d MMMM yyyy", new CultureInfo("tr-TR")),
														  DepositAmount    = request.Type == 0 ? request.Amount : 0,
														  WithdrawalAmount = request.Type == 1 ? request.Amount : 0,
														  Description      = request.Description,
														  CashRegisterId   = request.CashRegisterId,
														  CashRegister     = cashRegister,
														  Opposite = new Dictionary<string, Guid?>() {
																		 {"CashRegister", request.CashRegisterDetailId},
																		 {"Bank", request.OppositeBankId}
																	 },
													  };
		await cashRegisterDetailRepository.AddAsync(cashRegisterDetail, cancellationToken);


		if (request.CashRegisterDetailId is not null) {
			CashRegister oppositeCashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.CashRegisterDetailId.Value, cancellationToken);

			decimal sourceToTargetRate;

			if (cashRegister.CurrencyType.Name == oppositeCashRegister.CurrencyType.Name) {
				sourceToTargetRate = 1;
			}
			else if (cashRegister.CurrencyType.Name == "TRY") {
				decimal forexRate = request.Type == 1 ?
					await GetCurrencyRate(oppositeCashRegister.CurrencyType.Name, "ForexBuying") :
					await GetCurrencyRate(oppositeCashRegister.CurrencyType.Name, "ForexSelling");

				sourceToTargetRate = 1 / forexRate;
			} else if (oppositeCashRegister.CurrencyType.Name == "TRY") {
				decimal forexRate = request.Type == 1 ?
					await GetCurrencyRate(cashRegister.CurrencyType.Name, "ForexBuying") :
					await GetCurrencyRate(cashRegister.CurrencyType.Name, "ForexSelling");
				sourceToTargetRate = forexRate;
			}
			else
			{
				decimal sourceCurrencyToTRY = await GetCurrencyRate(cashRegister.CurrencyType.Name, request.Type == 1 ? "ForexBuying" : "ForexSelling");
				decimal targetCurrencyToTRY = await GetCurrencyRate(oppositeCashRegister.CurrencyType.Name, request.Type == 1 ? "ForexBuying" : "ForexSelling");
				sourceToTargetRate = sourceCurrencyToTRY / targetCurrencyToTRY;
			}

			oppositeCashRegister.DepositAmount    += (request.Type == 1 ? request.Amount * sourceToTargetRate : 0);
			oppositeCashRegister.WithdrawalAmount += (request.Type == 0 ? request.Amount * sourceToTargetRate : 0);
			oppositeCashRegister.BalanceAmount += (request.Type == 1 ? request.Amount * sourceToTargetRate : 0) -
												  (request.Type == 0 ? request.Amount * sourceToTargetRate : 0);

			CashRegisterDetail oppositeCashRegisterDetail = new() {
																	  Processor = new Dictionary<string, Guid> {
																		  { userName, Guid.Parse(userId) }
																	  },
																	  Date             = request.Date.ToString("d MMMM yyyy", new CultureInfo("tr-TR")),
																	  DepositAmount    = request.Type == 1 ? request.Amount * sourceToTargetRate : 0,
																	  WithdrawalAmount = request.Type == 0 ? request.Amount * sourceToTargetRate : 0,
																	  Description      = request.Description,
																	  CashRegisterId   = request.CashRegisterDetailId.Value,
																	  CashRegister     = oppositeCashRegister,
																	  Opposite = new Dictionary<string, Guid?>() {
																		  {"CashRegister", request.CashRegisterId},
																		  {"Bank", null}
																	  },
																  };
			await cashRegisterDetailRepository.AddAsync(oppositeCashRegisterDetail, cancellationToken);
		}

		if (request.OppositeBankId is not null) {
			Bank oppositeBank = await bankRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.OppositeBankId.Value, cancellationToken);

			decimal sourceToTargetRate;

			if (cashRegister.CurrencyType.Name == oppositeBank.CurrencyType.Name) {
				sourceToTargetRate = 1;
			} else if (cashRegister.CurrencyType.Name == "TRY") {
				decimal forexRate = request.Type == 1 ?
					await GetCurrencyRate(oppositeBank.CurrencyType.Name, "ForexBuying") :
					await GetCurrencyRate(oppositeBank.CurrencyType.Name, "ForexSelling");

				sourceToTargetRate = 1 / forexRate;
			} else if (oppositeBank.CurrencyType.Name == "TRY") {
				decimal forexRate = request.Type == 1 ?
					await GetCurrencyRate(cashRegister.CurrencyType.Name, "ForexBuying") :
					await GetCurrencyRate(cashRegister.CurrencyType.Name, "ForexSelling");

				sourceToTargetRate = forexRate;
			}
			else {
				decimal sourceCurrencyToTRY = await GetCurrencyRate(cashRegister.CurrencyType.Name, request.Type == 1 ? "ForexBuying" : "ForexSelling");
				decimal targetCurrencyToTRY = await GetCurrencyRate(oppositeBank.CurrencyType.Name, request.Type == 1 ? "ForexBuying" : "ForexSelling");

				sourceToTargetRate = sourceCurrencyToTRY / targetCurrencyToTRY;
			}

			oppositeBank.DepositAmount  += (request.Type == 1 ? request.Amount * sourceToTargetRate : 0);
			oppositeBank.WithdrawAmount += (request.Type == 0 ? request.Amount * sourceToTargetRate : 0);
			oppositeBank.Balance += (request.Type == 1 ? request.Amount * sourceToTargetRate : 0) -
									(request.Type == 0 ? request.Amount * sourceToTargetRate : 0);

			BankDetail oppositeBankDetail = new() {
													  Processor = new Dictionary<string, Guid> {
																		  { userName, Guid.Parse(userId) }
																	  },
													  Date = request.Date.ToString("d MMMM yyyy", new CultureInfo("tr-TR")),
													  DepositAmount = request.Type == 1
														  ? request.Amount * sourceToTargetRate
														  : 0,
													  WithdrawalAmount = request.Type == 0
														  ? request.Amount * sourceToTargetRate
														  : 0,
													  Description    = request.Description,
													  BankId         = request.OppositeBankId.Value,
													  Bank           = oppositeBank,
													  Opposite = new Dictionary<string, Guid?>() {
																	 {"CashRegister", request.CashRegisterId},
																	 {"Bank", null}
																 },
												  };

			await bankDetailRepository.AddAsync(oppositeBankDetail, cancellationToken);
		}

		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return "Cash register detail created successfully";
	}

	async Task<decimal> GetCurrencyRate(string currencyType, string currencyType2)
	{
		using (var client = new HttpClient())
		{
			string url = "https://www.tcmb.gov.tr/kurlar/today.xml";

			var response = await client.GetStringAsync(url);

			if (string.IsNullOrEmpty(response))
				return 1;

			XDocument doc = XDocument.Parse(response);

			var currency = doc.Descendants("Currency");


			foreach (var item in currency) {
				string? kod          = item.Attribute("Kod")?.Value;

				if (kod != currencyType)
					continue;

				string? value  = item.Element(currencyType2)?.Value;

				string[] split = value!.Split(',');

				if (split.Length == 1)
				{
					var num = decimal.Parse(split[0], System.Globalization.CultureInfo.InvariantCulture);
					return num;
				}
				else if (split.Length == 2)
				{
					var num = decimal.Parse(value, System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));
					return num;
				}
			}
		}

		return 0;
	}
}
