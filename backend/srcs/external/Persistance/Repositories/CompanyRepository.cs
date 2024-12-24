using Domain.Entities;
using Domain.Repositories;
using GenericRepository;
using Persistance.Contexts.ApplicationDb;

namespace Persistance.Repositories;

internal sealed class CompanyRepository(
	AppDbContext context)
	: Repository<Company, AppDbContext>(context), ICompanyRepository;