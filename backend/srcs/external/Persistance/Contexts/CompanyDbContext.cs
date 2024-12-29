using System.Security.Claims;
using Domain.Entities;
using Domain.Entities.CompanyEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistance.Contexts.ApplicationDb;
using Persistance.Services;

namespace Persistance.Contexts;

public sealed class CompanyDbContext : DbContext, IUnitOfWorkCompany {
    #region Connection
    private string connectionString = string.Empty;

    public CompanyDbContext(Company company) {
        CreateConnectionStringWithCompany(company);
    }

    public CompanyDbContext(IHttpContextAccessor httpContextAccessor, AppDbContext context) {
        CreateConnectionString(httpContextAccessor, context);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(connectionString);
    }

    private void CreateConnectionString(IHttpContextAccessor httpContextAccessor, AppDbContext context) {
        if (httpContextAccessor.HttpContext is null) return;

        var companyId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(companyId)) return;

        Company? company = context.Companies.Find(Guid.Parse(companyId));
        if (company is null) return;

        CreateConnectionStringWithCompany(company);
    }

    private void CreateConnectionStringWithCompany(Company company) {
        if (string.IsNullOrEmpty(company.Database.UserId)) {
            connectionString =
                $"Data Source={company.Database.Server};"           +
                $"Initial Catalog={company.Database.DatabaseName};" +
                "Integrated Security=True;"                         +
                "Connect Timeout=30;"                               +
                "Encrypt=True;"                                     +
                "Trust Server Certificate=True;"                    +
                "Application Intent=ReadWrite;"                     +
                "Multi Subnet Failover=False";
        } else {
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
    #endregion

    public DbSet<CashRegister> CashRegisters { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}