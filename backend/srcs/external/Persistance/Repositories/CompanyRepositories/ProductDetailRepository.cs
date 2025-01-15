using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class ProductDetailRepository(CompanyDbContext context)
	: Repository<ProductDetail, CompanyDbContext>(context), IProductDetailRepository;