using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class BankRepository(CompanyDbContext context)
	: Repository<Bank, CompanyDbContext>(context), IBankRepository;