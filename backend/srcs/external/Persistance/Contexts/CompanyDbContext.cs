using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Services;

namespace Persistance.Contexts;

public sealed class CompanyDbContext : DbContext, IUnitOfWorkCompany {
	private string connectionString = string.Empty;

	public CompanyDbContext(Company company) {
		CreateConnectionStringWithCompany(company);
	}

	public CompanyDbContext(DbContextOptions options) : base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		optionsBuilder.UseSqlServer(connectionString);
	}

	private void CreateConnectionStringWithCompany(Company company)
	{
		if (string.IsNullOrEmpty(company.Database.UserId))
		{
			connectionString =
				$"Data Source={company.Database.Server};"           +
				$"Initial Catalog={company.Database.DatabaseName};" +
				"Integrated Security=True;"                         +
				"Connect Timeout=30;"                               +
				"Encrypt=True;"                                     +
				"Trust Server Certificate=True;"                    +
				"Application Intent=ReadWrite;"                     +
				"Multi Subnet Failover=False";
		}
		else
		{
			connectionString =
				$"Data Source={company.Database.Server};"           +
				$"Initial Catalog={company.Database.DatabaseName};" +
				"Integrated Security=False;"                        +
				$"User Id={company.Database.UserId};"               +
				$"Password={company.Database.Password};"            +
				"Connect Timeout=30;"                               +
				"Encrypt=True;"                                     +
				"Trust Server Certificate=True;"                    +
				"Application Intent=ReadWrite;"                     +
				"Multi Subnet Failover=False";
		}
	}
}