using System.Globalization;
using System.Security.Claims;
using System.Xml.Linq;
using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.BankDetails.BankDetailCreate;

internal sealed record BankDetailCreateHandler(
	IBankRepository       bankRepository,
	IBankDetailRepository bankDetailRepository,
	ICashRegisterRepository cashRegisterRepository,
	ICashRegisterDetailRepository cashRegisterDetailRepository,
	IUnitOfWorkCompany    unitOfWorkCompany,
	IHttpContextAccessor  httpContextAccessor) : IRequestHandler<BankDetailCreateRequest, Result<string>> {
	public async Task<Result<string>> Handle(BankDetailCreateRequest request, CancellationToken cancellationToken) {
		if (httpContextAccessor.HttpContext is null)
			return (500, "You are not authorized to do this");

		string? userId   = httpContextAccessor.HttpContext.User.FindFirstValue("Id");
		string? userName = httpContextAccessor.HttpContext.User.FindFirstValue("Username");

		if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
			return (500, "User not found");

		Bank? bank = await bankRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.BankId, cancellationToken);

		bank.DepositAmount  += (request.Type == 0 ? request.Amount : 0);
		bank.WithdrawAmount += (request.Type == 1 ? request.Amount : 0);
		bank.Balance        += (request.Type == 0 ? request.Amount : 0) - (request.Type == 1 ? request.Amount : 0);

		BankDetail bankDetail = new() {
										  Processor = new Dictionary<string, Guid> {
															  { userName, Guid.Parse(userId) }
														  },
										  Date             = request.Date.ToString("d MMMM yyyy", new CultureInfo("tr-TR")),
										  DepositAmount    = request.Type == 0 ? request.Amount : 0,
										  WithdrawalAmount = request.Type == 1 ? request.Amount : 0,
										  Description      = request.Description,
										  BankId           = request.BankId,
										  Bank             = bank,
										  Opposite = new Dictionary<string, Guid?>() {
														 {"CashRegister", request.OppositeCashRegisterId},
														 {"Bank", request.OppositeBankId}
													 },
									  };
		await bankDetailRepository.AddAsync(bankDetail, cancellationToken);

		if (request.OppositeBankId is not null) {
			Bank oppositeBank = await bankRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.OppositeBankId.Value, cancellationToken);

			decimal sourceToTargetRate;

			if (bank.CurrencyType.Name == oppositeBank.CurrencyType.Name) {
				sourceToTargetRate = 1;
			} else if (bank.CurrencyType.Name == "TRY") {
				decimal forexRate = request.Type == 1 ?
					await GetCurrencyRate(oppositeBank.CurrencyType.Name, "ForexBuying") :
					await GetCurrencyRate(oppositeBank.CurrencyType.Name, "ForexSelling");

				sourceToTargetRate = 1 / forexRate;
			} else if (oppositeBank.CurrencyType.Name == "TRY") {
				decimal forexRate = request.Type == 1 ?
					await GetCurrencyRate(bank.CurrencyType.Name, "ForexBuying") :
					await GetCurrencyRate(bank.CurrencyType.Name, "ForexSelling");

				sourceToTargetRate = forexRate;
			}
			else {
				decimal sourceCurrencyToTRY = await GetCurrencyRate(bank.CurrencyType.Name, request.Type == 1 ? "ForexBuying" : "ForexSelling");
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
																	 {"CashRegister", null},
																	 {"Bank", request.BankId}
																 },
												  };

			await bankDetailRepository.AddAsync(oppositeBankDetail, cancellationToken);
		}

		if (request.OppositeCashRegisterId is not null) {
			CashRegister oppositeCashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.OppositeCashRegisterId.Value, cancellationToken);

			decimal sourceToTargetRate;

			if (bank.CurrencyType.Name == oppositeCashRegister.CurrencyType.Name) {
				sourceToTargetRate = 1;
			}
			else if (bank.CurrencyType.Name == "TRY") {
				decimal forexRate = request.Type == 1 ?
					await GetCurrencyRate(oppositeCashRegister.CurrencyType.Name, "ForexBuying") :
					await GetCurrencyRate(oppositeCashRegister.CurrencyType.Name, "ForexSelling");

				sourceToTargetRate = 1 / forexRate;
			} else if (oppositeCashRegister.CurrencyType.Name == "TRY") {
				// Diğer para biriminden TRY'ye çevrim
				decimal forexRate = request.Type == 1 ?
					await GetCurrencyRate(bank.CurrencyType.Name, "ForexBuying") :
					await GetCurrencyRate(bank.CurrencyType.Name, "ForexSelling");
				sourceToTargetRate = forexRate;
			}
			else
			{
				// USD-EUR gibi çapraz kur hesaplaması
				decimal sourceCurrencyToTRY = await GetCurrencyRate(bank.CurrencyType.Name, request.Type == 1 ? "ForexBuying" : "ForexSelling");
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
																	  CashRegisterId   = request.OppositeCashRegisterId.Value,
																	  CashRegister     = oppositeCashRegister,
																	  Opposite = new Dictionary<string, Guid?>() {
																		  {"CashRegister", null},
																		  {"Bank", bank.Id}
																	  },
																  };
			await cashRegisterDetailRepository.AddAsync(oppositeCashRegisterDetail, cancellationToken);
		}

		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return "Bank detail created successfully";
	}
	async Task<decimal> GetCurrencyRate(string currencyType, string currencyType2)
	{
		using (var client = new HttpClient())
		{
			string url = "https://www.tcmb.gov.tr/kurlar/today.xml";

			var response = await client.GetStringAsync(url);

			XDocument doc = XDocument.Parse(response);

			var currencies = doc.Descendants("Currency");

			var currency = currencies.FirstOrDefault(c => c.Attribute("Kod")?.Value == currencyType);

			if (currency is not null)
			{
				string element = currency.Element(currencyType2)!.Value;

				string[] split = element.Split(".");

				return decimal.Parse(split[0] + "," + split[1]);
			}
		}

		return 0;
	}
}