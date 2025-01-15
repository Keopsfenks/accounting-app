using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class CashRegisterDetailRepository(CompanyDbContext context)
	: Repository<CashRegisterDetail, CompanyDbContext>(context), ICashRegisterDetailRepository;