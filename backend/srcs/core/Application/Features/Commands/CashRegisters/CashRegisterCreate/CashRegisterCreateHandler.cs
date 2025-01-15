using AutoMapper;
using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.CashRegisters.CashRegisterCreate;

internal sealed record CashRegisterCreateHandler(
	ICashRegisterRepository cashRegisterRepository,
	IUnitOfWorkCompany      unitOfWorkCompany,
	IMapper                 mapper) : IRequestHandler<CashRegisterCreateRequest, Result<string>> {

	public async Task<Result<string>> Handle(CashRegisterCreateRequest createRequest, CancellationToken cancellationToken) {
		bool isNameExist = await cashRegisterRepository.AnyAsync(cr => cr.Name == createRequest.Name);

		if (isNameExist) {
			return Result<string>.Failure("Cash register already exist");
		}

		CashRegister cashRegister = mapper.Map<CashRegister>(createRequest);
		await cashRegisterRepository.AddAsync(cashRegister, cancellationToken);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return "Cash register created successfully";
	}
}