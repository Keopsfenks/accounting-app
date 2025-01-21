using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class ProductRepository(CompanyDbContext context)
	: Repository<Product, CompanyDbContext>(context), IProductRepository;