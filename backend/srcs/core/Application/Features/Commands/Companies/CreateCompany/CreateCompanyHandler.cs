using Application.Services.Companies;
using AutoMapper;
using Domain.Entities;
using Domain.Events;
using Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Commands.Companies.CreateCompany;

public sealed record CreateCompanyHandler(
	ICompanyRepository companyRepository,
	ICompanyUserRepository companyUserRepository,
	IHttpContextAccessor httpContextAccessor,
	IUnitOfWork        unitOfWork,
	IMapper            mapper,
	IMediator		  mediator,
	ICompanyService    companyService) : IRequestHandler<CreateCompanyRequest, Result<string>> {
	public async Task<Result<string>> Handle(CreateCompanyRequest request, CancellationToken cancellationToken) {
		bool isTaxNumberExists = await companyRepository.AnyAsync(p=> p.TaxId  == request.TaxId, cancellationToken);

		string? userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
		if (string.IsNullOrEmpty(userId)) {
			return Result<string>.Failure("User not found");
		}

		if (isTaxNumberExists) {
			return Result<string>.Failure("Tax number already exists");
		}

		Company company = mapper.Map<Company>(request);
		await companyRepository.AddAsync(company, cancellationToken);

		await unitOfWork.SaveChangesAsync(cancellationToken);

		companyService.MigrateCompanyDatabase(company);
		try {
			await mediator.Publish(new CompanyEvent(userId, company.Id, "create"), cancellationToken);
		}
		catch (Exception e) {
			Result<string>.Failure(e.Message);
			throw;
		}

		return "Company created successfully";
	}
}