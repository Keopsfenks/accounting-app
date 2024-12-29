using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public class CashRegisterRepositories(CompanyDbContext context)
	: Repository<CashRegister, CompanyDbContext>(context), ICashRegisterRepository;