using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class BankDetailRepository(CompanyDbContext context)
	: Repository<BankDetail, CompanyDbContext>(context), IBankDetailRepository;