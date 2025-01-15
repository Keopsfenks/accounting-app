using System.Security.Claims;
using System.Text.Json;
using Domain.Entities;
using Domain.Entities.CompanyEntities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistance.Contexts.ApplicationDb;
using Persistance.Services;

namespace Persistance.Contexts;

public sealed class CompanyDbContext : DbContext, IUnitOfWorkCompany {
    #region Connection
    private string connectionString = string.Empty;

    public CompanyDbContext(Company company)
    {
        CreateConnectionStringWithCompany(company);
    }

    public CompanyDbContext(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    {
        CreateConnectionString(httpContextAccessor, context);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }

    private void CreateConnectionString(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    {
        if (httpContextAccessor.HttpContext is null) return;

        string? companyId = httpContextAccessor.HttpContext.User.FindFirstValue("CompanyId");
        if (string.IsNullOrEmpty(companyId)) return;

        Company? company = context.Companies.Find(Guid.Parse(companyId));
        if (company is null) return;

        CreateConnectionStringWithCompany(company);
    }

    private void CreateConnectionStringWithCompany(Company company)
    {
        if (string.IsNullOrEmpty(company.Database.UserId))
        {
            connectionString =
            $"Data Source={company.Database.Server};" +
            $"Initial Catalog={company.Database.DatabaseName};" +
            "Integrated Security=True;" +
            "Connect Timeout=30;" +
            "Encrypt=True;" +
            "Trust Server Certificate=True;" +
            "Application Intent=ReadWrite;" +
            "Multi Subnet Failover=False";
        }
        else
        {
            connectionString =
            $"Data Source={company.Database.Server};" +
            $"Initial Catalog={company.Database.DatabaseName};" +
            "Integrated Security=False;" +
            $"User Id={company.Database.UserId};" +
            $"Password={company.Database.Password};" +
            "Connect Timeout=30;" +
            "Encrypt=True;" +
            "Trust Server Certificate=True;" +
            "Application Intent=ReadWrite;" +
            "Multi Subnet Failover=False";
        }
    }
    #endregion

    public DbSet<CashRegister>       CashRegisters       { get; set; }
    public DbSet<CashRegisterDetail> CashRegisterDetails { get; set; }
    public DbSet<Bank>               Banks               { get; set; }
    public DbSet<BankDetail>         BankDetails         { get; set; }
	public DbSet<Category>           Categories          { get; set; }
	public DbSet<Product>            Products            { get; set; }
	public DbSet<ProductDetail>      ProductDetails      { get; set; }
	public DbSet<Customer>           Customers           { get; set; }
	public DbSet<CustomerDetail>     CustomersDetails    { get; set; }
	public DbSet<Invoice>            Invoices            { get; set; }
	public DbSet<CashProceeds>       CashProceeds        { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
	#region CashRegister
        modelBuilder.Entity<CashRegister>(builder => {
            builder.ToTable("CashRegister");
            builder.Property(p => p.DepositAmount).HasColumnType("money");
            builder.Property(p => p.WithdrawalAmount).HasColumnType("money");
            builder.Property(p => p.BalanceAmount).HasColumnType("money");

            builder.Property(p => p.CurrencyType)
                   .HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));

            builder.HasMany(cr => cr.Details)
                   .WithOne(d => d.CashRegister)
                   .HasForeignKey(d => d.CashRegisterId)
                   .OnDelete(DeleteBehavior.Cascade);
        });
    #endregion
    #region CashRegisterDetail

        modelBuilder.Entity<CashRegisterDetail>(builder => {
            builder.ToTable("CashRegisterDetail");
            builder.Property(p => p.DepositAmount).HasColumnType("money");
            builder.Property(p => p.WithdrawalAmount).HasColumnType("money");

            builder.Property(p => p.Processor)
                   .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                        v => JsonSerializer.Deserialize<Dictionary<string, Guid>>(v, (JsonSerializerOptions)null!)!
                    );

            builder.Property(p => p.Opposite)
                   .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                        v => JsonSerializer.Deserialize<Dictionary<string, Guid?>>(v, (JsonSerializerOptions)null!)!
                    );

            builder.HasOne(crd => crd.CashRegister)
                   .WithMany(cr => cr.Details)
                   .HasForeignKey(crd => crd.CashRegisterId)
                   .OnDelete(DeleteBehavior.Cascade);
        });
    #endregion
    #region Bank

        modelBuilder.Entity<Bank>(builder => {
            builder.ToTable("Bank");
            builder.Property(p => p.DepositAmount).HasColumnType("money");
            builder.Property(p => p.WithdrawAmount).HasColumnType("money");
            builder.Property(p => p.Balance).HasColumnType("money");

            builder.Property(p => p.CurrencyType)
                   .HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));

            builder.HasMany(p => p.Details)
                   .WithOne(b => b.Bank)
                   .HasForeignKey(b => b.BankId)
                   .OnDelete(DeleteBehavior.Cascade);
        });


    #endregion
    #region BankDetail

        modelBuilder.Entity<BankDetail>(builder => {
            builder.ToTable("BankDetail");
            builder.Property(p => p.DepositAmount).HasColumnType("money");
            builder.Property(p => p.WithdrawalAmount).HasColumnType("money");

            builder.Property(p => p.Processor)
                   .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                        v => JsonSerializer.Deserialize<Dictionary<string, Guid>>(v, (JsonSerializerOptions)null!)!
                    );


            builder.Property(p => p.Opposite)
                   .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                        v => JsonSerializer.Deserialize<Dictionary<string, Guid?>>(v, (JsonSerializerOptions)null!)!
                    );

            builder.HasOne(crd => crd.Bank)
                   .WithMany(cr => cr.Details)
                   .HasForeignKey(crd => crd.BankId)
                   .OnDelete(DeleteBehavior.Cascade);
        });


    #endregion
	/*#region Product
        modelBuilder.Entity<Product>(builder => {
            builder.ToTable("Product");
            builder.Property(p => p.Deposit).HasColumnType("decimal(7,2)");
            builder.Property(p => p.Withdrawal).HasColumnType("decimal(7,2)");
            builder.HasKey(x => x.Id);

            builder.HasOne(p => p.Category)
                   .WithMany()
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);


            builder.OwnsOne(p => p.Metadata, childBuilder => {
                childBuilder.WithOwner();
                childBuilder.Property(p => p.Barcode).HasColumnType("nvarchar(50)").IsRequired(false);
                childBuilder.Property(p => p.Image).HasColumnType("nvarchar(50)").IsRequired(false);
                childBuilder.Property(p => p.Supplier).HasColumnType("nvarchar(50)").IsRequired(false);
                childBuilder.Property(p => p.SKU).HasColumnType("nvarchar(50)").IsRequired(false);
            });

            builder.OwnsOne(p => p.Attributes, childBuilder => {
                childBuilder.WithOwner();
                childBuilder.Property(p => p.Brand).HasColumnType("nvarchar(50)").IsRequired(false);
                childBuilder.Property(p => p.Dimensions).HasColumnType("nvarchar(50)").IsRequired(false);
                childBuilder.Property(p => p.Weight).HasColumnType("nvarchar(50)").IsRequired(false);
                childBuilder.Property(p => p.AdditionalAttributes)
                            .HasColumnType("nvarchar(max)")
                            .HasConversion(
                                 v => v != null ? JsonSerializer.Serialize(v, new JsonSerializerOptions()) : null,
                                 v => v != null ? JsonSerializer.Deserialize<Dictionary<string, string>>(v, new JsonSerializerOptions()) : null
                             )
                            .IsRequired(false);
            });

            builder.OwnsOne(p => p.Stock, childBuilder => {
                childBuilder.WithOwner();
                childBuilder.Property(p => p.StockQuantity).HasColumnType("decimal(18,2)");
				childBuilder.Property(p => p.UnitOfMeasure)
							.HasConversion(type => type.Value, value => ProductUnitOfMeasureEnum.FromValue(value));
            });

            builder.HasMany(p => p.Operations)
                   .WithOne(c => c.Product)
                   .HasForeignKey(c => c.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        });
    #endregion*/
    #region Category
        modelBuilder.Entity<Category>(builder =>
        {
            builder.ToTable("Category");
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.ParentCategoryId);

            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Category)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.ParentCategory)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(c => c.ParentCategoryId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);
        });
    #endregion
	#region ProductDetail
		modelBuilder.Entity<ProductDetail>(builder => {
			builder.ToTable("ProductDetail");

			builder.Property(p => p.Processor)
				   .HasConversion(
						v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
						v => JsonSerializer.Deserialize<Dictionary<string, Guid>>(v, (JsonSerializerOptions)null!)!
					);

			builder.Property(p => p.Type)
				   .HasConversion(type => type.Value, value => OperationTypeEnum.FromValue(value));


			builder.OwnsOne(p => p.Pricing, childBuilder => {
				childBuilder.WithOwner();
				childBuilder.Property(p => p.Quantity).HasColumnType("decimal(18,2)");
				childBuilder.Property(p => p.UnitPrice).HasColumnType("money");
				childBuilder.Property(p => p.TaxRate).HasColumnType("money");
				childBuilder.Property(p => p.TotalPrice).HasColumnType("money");
				childBuilder.Property(p => p.DiscountRate).HasColumnType("nvarchar(50)");
			});

			builder.HasOne(c => c.Product)
				   .WithMany(c => c.Operations)
				   .HasForeignKey(c => c.ProductId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(c => c.Invoice)
				   .WithMany(c => c.Products)
				   .HasForeignKey(c => c.InvoiceId)
				   .IsRequired(false)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(c => c.CustomerDetail)
				   .WithMany(c => c.Products)
				   .HasForeignKey(c => c.CustomerDetailId)
				   .IsRequired(false)
				   .OnDelete(DeleteBehavior.Cascade);

		});
	#endregion
		#region Customer

		modelBuilder.Entity<Customer>(builder => {
			builder.ToTable("Customer");
			builder.Property(p => p.Deposit).HasColumnType("money");
			builder.Property(p => p.Withdrawal).HasColumnType("money");
			builder.Property(p => p.Debit).HasColumnType("money");

			builder.Property(p => p.Type)
				   .HasConversion(type => type.Value, value => CustomerTypeEnum.FromValue(value));

			builder.HasMany(c => c.Details)
				   .WithOne(c => c.Customer)
				   .HasForeignKey(c => c.CustomerId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(c => c.Invoices)
				   .WithOne(c => c.Customer)
				   .HasForeignKey(c => c.CustomerId)
				   .OnDelete(DeleteBehavior.Cascade);
		});


	#endregion
	#region CustomerDetail
		modelBuilder.Entity<CustomerDetail>(builder => {
			builder.ToTable("CustomerDetail");
			builder.Property(p => p.Processor)
				   .HasConversion(
						v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
						v => JsonSerializer.Deserialize<Dictionary<string, Guid>>(v, (JsonSerializerOptions)null!)!
					);

			builder.Property(p => p.Opposite)
				   .HasConversion(
						v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
						v => JsonSerializer.Deserialize<Dictionary<string, Guid?>>(v, (JsonSerializerOptions)null!)!
					);

			builder.Property(p => p.DepositAmount).HasColumnType("money");
			builder.Property(p => p.WithdrawalAmount).HasColumnType("money");
			builder.Property(p => p.TotalAmount).HasColumnType("money");
			builder.Property(p => p.IssueDate).HasColumnType("datetime2");
			builder.Property(p => p.DueDate)?.HasColumnType("datetime2").IsRequired(false);

			builder.Property(p => p.Payment)
				   .HasConversion(type => type.Value, value => PaymentEnum.FromValue(value));
			builder.Property(p => p.Operation)
				   .HasConversion(type => type.Value, value => OperationTypeEnum.FromValue(value));
			builder.Property(p => p.Status)
				   .HasConversion(type => type.Value, value => StatusEnum.FromValue(value));

			builder.HasMany(c => c.Products)
				   .WithOne(p => p.CustomerDetail)
				   .HasForeignKey(p => p.CustomerDetailId)
				   .OnDelete(DeleteBehavior.Cascade);
			builder.HasMany(c => c.CashProceeds)
				   .WithOne(p => p.CustomerDetail)
				   .HasForeignKey(p => p.CustomerDetailId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(c => c.Customer)
				   .WithMany(c => c.Details)
				   .HasForeignKey(c => c.CustomerId)
				   .OnDelete(DeleteBehavior.NoAction);

		});

	#endregion
    #region Invoice
        modelBuilder.Entity<Invoice>(builder => {
            builder.ToTable("Invoice");
			builder.Property(p => p.Processor)
				   .HasConversion(
						v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
						v => JsonSerializer.Deserialize<Dictionary<string, Guid>>(v, (JsonSerializerOptions)null!)!
					);
            builder.Property(p => p.DepositAmount).HasColumnType("money");
            builder.Property(p => p.WithdrawalAmount).HasColumnType("money");
            builder.Property(p => p.TotalAmount).HasColumnType("money");
            builder.Property(p => p.IssueDate).HasColumnType("datetime2");
            builder.Property(p => p.DueDate)?.HasColumnType("datetime2").IsRequired(false);

            builder.Property(p => p.CurrencyType)
                   .HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
            builder.Property(p => p.Payment)
                   .HasConversion(type => type.Value, value => PaymentEnum.FromValue(value));
            builder.Property(p => p.Operation)
                   .HasConversion(type => type.Value, value => OperationTypeEnum.FromValue(value));
            builder.Property(p => p.Status)
                   .HasConversion(type => type.Value, value => StatusEnum.FromValue(value));

            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Invoice)
                   .HasForeignKey(p => p.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.CashProceeds)
                   .WithOne(p => p.Invoice)
                   .HasForeignKey(p => p.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Customer)
                   .WithMany(c => c.Invoices)
                   .HasForeignKey(c => c.CustomerId)
                   .OnDelete(DeleteBehavior.NoAction);
        });

    #endregion
	#region CashProceeds
		modelBuilder.Entity<CashProceeds>(builder => {
			builder.ToTable("CashProceeds");
			builder.Property(p => p.Amount).HasColumnType("money");
			builder.Property(p => p.IssueDate).HasColumnType("datetime2");
			builder.Property(p => p.Operation)
				   .HasConversion(type => type.Value, value => OperationTypeEnum.FromValue(value));
			builder.Property(p => p.PaymentType)
				   .HasConversion(type => type.Value, value => PaymentTypeEnum.FromValue(value));


			builder.OwnsOne(c => c.Cheque, childBuilder => {
				childBuilder.Property(property => property.BankName).HasColumnName("BankName");
				childBuilder.Property(property => property.ChequeNumber).HasColumnName("ChequeNumber");
				childBuilder.Property(property => property.MaturityDate).HasColumnName("MaturityDate").HasColumnType("datetime2");
			});

			builder.HasOne(p => p.CustomerDetail)
				   .WithMany(p => p.CashProceeds)
				   .HasForeignKey(c => c.CustomerDetailId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(p => p.Invoice)
				   .WithMany(c => c.CashProceeds)
				   .HasForeignKey(c => c.InvoiceId)
				   .OnDelete(DeleteBehavior.Cascade);

		});
	#endregion
    }
}