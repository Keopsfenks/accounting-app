using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class CustomerRepository(CompanyDbContext context)
	: Repository<Customer, CompanyDbContext>(context), ICustomerRepository;