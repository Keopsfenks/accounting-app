using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Customers;

public sealed record GetIdToCustomer() : IRequest<Result<List<Customer>>> {
	public Guid Id { get; set; } = Guid.Empty;
}



internal sealed record GetIdtoCustomerHandler(
	ICustomerRepository customerRepository) : IRequestHandler<GetIdToCustomer, Result<List<Customer>>> {
	public async Task<Result<List<Customer>>> Handle(GetIdToCustomer request, CancellationToken cancellationToken) {
		Guid Id = request.Id;

		List<Customer> customers = await customerRepository.GetAll()
														   .Where(c => c.Id == Id)
														   .OrderBy(c => c.Name)
														   .Include(c => c.Details)
														   .ThenInclude(c => c.Products)
														   .Include(c => c.Details)
														   .ThenInclude(c => c.CashProceeds)
														   .Include(c => c.Invoices)
														   .ThenInclude(c => c.Products)
														   .Include(c => c.Invoices)
														   .ThenInclude(c => c.CashProceeds)
														   .ToListAsync(cancellationToken);

		return customers;
	}
}