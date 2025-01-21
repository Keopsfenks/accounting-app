using AutoMapper;
using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Banks.BankCreate;

internal sealed class BankCreateHandler(
	IBankRepository    bankRepository,
	IUnitOfWorkCompany unitOfWorkCompany,
	IMapper            mapper) : IRequestHandler<BankCreateRequest, Result<string>> {
	public async Task<Result<string>> Handle(BankCreateRequest request, CancellationToken cancellationToken) {
		bool isIBANExists = await bankRepository.AnyAsync(p => p.Iban == request.Iban, cancellationToken);

		if (isIBANExists)
			return (500, "IBAN already exists");

		Bank bank = mapper.Map<Bank>(request);

		await bankRepository.AddAsync(bank, cancellationToken);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
		
		return "Bank created successfully";
	}
}