using Application.Services.Companies;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Contexts;

namespace Persistance.Services;

internal sealed class CompanyService : ICompanyService
{
	public void MigrateCompanyDatabase(Company company)
	{
		using var context = new CompanyDbContext(company);
		context.Database.EnsureCreated();

		if (context.Database.GetPendingMigrations().Any())
		{
			context.Database.Migrate();
		}
	}
}