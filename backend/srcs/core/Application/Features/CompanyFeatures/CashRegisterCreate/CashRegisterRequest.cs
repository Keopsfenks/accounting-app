using Application.Services.Companies;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.CompanyEntities;
using Domain.Repositories;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.Services;
using TS.Result;

namespace Application.Features.CompanyFeatures.CashRegisterCreate;

public sealed record CashRegisterRequest(
	string Name,
	int CurrencyType) : IRequest<Result<string>>;

internal sealed record CashRegisterHandler(
	ICashRegisterRepository cashRegisterRepository,
	IUnitOfWorkCompany      unitOfWorkCompany,
	IMapper                 mapper) : IRequestHandler<CashRegisterRequest, Result<string>> {

	public async Task<Result<string>> Handle(CashRegisterRequest request, CancellationToken cancellationToken) {
		bool isNameExist = await cashRegisterRepository.AnyAsync(cr => cr.Name == request.Name);

		if (isNameExist) {
			return Result<string>.Failure("Cash register already exist");
		}

		CashRegister cashRegister = mapper.Map<CashRegister>(request);
		await cashRegisterRepository.AddAsync(cashRegister, cancellationToken);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return "Cash register created successfully";
	}
}