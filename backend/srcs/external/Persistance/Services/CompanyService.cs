using Application.Services.Companies;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Contexts;

namespace Persistance.Services;

internal sealed class CompanyService : ICompanyService {
	public void MigrateCompanyDatabase(Company company) {
		CompanyDbContext context = new(company);
		context.Database.Migrate();
	}
}