using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class InvoiceRepository(CompanyDbContext context)
	: Repository<Invoice, CompanyDbContext>(context), IInvoiceRepository;