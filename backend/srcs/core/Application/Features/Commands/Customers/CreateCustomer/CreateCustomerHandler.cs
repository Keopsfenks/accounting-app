using AutoMapper;
using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Customers.CreateCustomer;

internal sealed record CreateCustomerHandler(
	ICustomerRepository customerRepository,
	IUnitOfWorkCompany  unitOfWorkCompany,
	IMapper             mapper) : IRequestHandler<CreateCustomerRequest, Result<string>> {
	public async Task<Result<string>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken) {
		Customer customer = mapper.Map<Customer>(request);

		await customerRepository.AddAsync(customer);
		await unitOfWorkCompany.SaveChangesAsync();

		return "Customer created successfully";

	}
}