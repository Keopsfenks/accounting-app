using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Customers.DeleteCustomer;

internal sealed record DeleteCustomerHandler(
	ICustomerRepository customerRepository,
	IUnitOfWorkCompany  unitOfWorkCompany) : IRequestHandler<DeleteCustomerRequest, Result<string>> {
	public async Task<Result<string>> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken) {
		Customer? customer = await customerRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
		
		if (customer is null) {
			return Result<string>.Failure("Customer not found");
		}
		
		customerRepository.Delete(customer);
		await unitOfWorkCompany.SaveChangesAsync();
		
		return "Customer deleted successfully";
	}
}