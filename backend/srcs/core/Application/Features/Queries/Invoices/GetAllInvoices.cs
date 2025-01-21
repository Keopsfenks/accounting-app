using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Invoices;

public sealed record GetAllInvoices() : IRequest<Result<List<Invoice>>> {
	public int PageNumber { get; set; } = 0;
	public int PageSize   { get; set; } = 10;
};


internal sealed record GetAllInvoicesHandler(
	IInvoiceRepository invoiceRepository) : IRequestHandler<GetAllInvoices, Result<List<Invoice>>> {
	public async Task<Result<List<Invoice>>> Handle(GetAllInvoices request, CancellationToken cancellationToken) {
		int pageNumber = request.PageNumber;
		int pageSize   = request.PageSize;

		List<Invoice> invoices = await invoiceRepository.GetAll()
														.OrderBy(i => i.InvoiceNumber)
														.Skip(pageNumber * pageSize)
														.Include(i => i.Products)
														.Include(i => i.CashProceeds)
														.Take(pageSize)
														.ToListAsync(cancellationToken);

		return invoices;
	}
}