using Domain.Entities;
using Domain.Repositories;
using GenericRepository;
using Persistance.Contexts.ApplicationDb;

namespace Persistance.Repositories;

internal sealed class CompanyUserRepository(
	AppDbContext context)
	: Repository<CompanyUsers, AppDbContext>(context), ICompanyUserRepository;