using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using GenericRepository;
using Persistance.Contexts;

namespace Persistance.Repositories.CompanyRepositories;

public sealed class CategoryRepository(CompanyDbContext context)
	: Repository<Category, CompanyDbContext>(context), ICategoryRepository;