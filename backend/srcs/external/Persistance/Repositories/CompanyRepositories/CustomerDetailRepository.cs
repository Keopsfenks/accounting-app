using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class CustomerDetailRepository(CompanyDbContext context)
	: Repository<CustomerDetail, CompanyDbContext>(context), ICustomerDetailRepository;