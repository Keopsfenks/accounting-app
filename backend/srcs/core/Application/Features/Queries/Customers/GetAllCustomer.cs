using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Customers;

public sealed record GetAllCustomer() : IRequest<Result<List<Customer>>> {
	public int PageNumber { get; set; } = 0;
	public int PageSize   { get; set; } = 10;
};



internal sealed record GetAllCustomerHandler(
	ICustomerRepository customerRepository) : IRequestHandler<GetAllCustomer, Result<List<Customer>>> {

	public async Task<Result<List<Customer>>> Handle(GetAllCustomer request, CancellationToken cancellationToken) {
		int pageNumber = request.PageNumber;
		int pageSize   = request.PageSize;

		List<Customer> customers = await customerRepository.GetAll()
														   .OrderBy(c => c.Name)
														   .Skip(pageNumber * pageSize)
														   .Include(c => c.Details)
														   .ThenInclude(c => c.Products)
														   .Include(c => c.Details)
														   .ThenInclude(c => c.CashProceeds)
														   .Include(c => c.Invoices)
														   .ThenInclude(c => c.Products)
														   .Include(c => c.Invoices)
														   .ThenInclude(c => c.CashProceeds)
														   .Take(pageSize)
														   .ToListAsync(cancellationToken);

		return customers;
	}
}