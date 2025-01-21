using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class CashProceedRepository(CompanyDbContext context)
	: Repository<CashProceed, CompanyDbContext>(context), ICashProceedRepository;