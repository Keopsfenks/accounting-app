using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.CustomerDetails;

public sealed record GetAllCustomerDetail() : IRequest<Result<List<CustomerDetail>>> {
	public int PageSize   { get; set; } = 10;
	public int PageNumber { get; set; } = 0;
}


internal sealed record GetAllCustomerDetailHandler(
	ICustomerDetailRepository customerDetailRepository) : IRequestHandler<GetAllCustomerDetail, Result<List<CustomerDetail>>> {
	public async Task<Result<List<CustomerDetail>>> Handle(GetAllCustomerDetail request, CancellationToken cancellationToken) {
		int pageNumber = request.PageNumber;
		int pageSize   = request.PageSize;

		List<CustomerDetail> customerDetails = await customerDetailRepository.GetAll()
		   .OrderBy(cr => cr.IssueDate)
		   .Skip(pageNumber * pageSize)
		   .Take(pageSize)
		   .ToListAsync(cancellationToken);

		return customerDetails;
	}
}