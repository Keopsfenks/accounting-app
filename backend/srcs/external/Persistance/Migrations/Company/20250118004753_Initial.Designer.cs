﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistance.Contexts;

#nullable disable

namespace Persistance.Migrations.Company
{
    [DbContext(typeof(CompanyDbContext))]
    [Migration("20250118004753_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Bank", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasColumnType("money");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CurrencyType")
                        .HasColumnType("int");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("money");

                    b.Property<string>("Iban")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("WithdrawAmount")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.ToTable("Bank", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.BankDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BankId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("money");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Opposite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Processor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("WithdrawalAmount")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.ToTable("BankDetail", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.CashProceed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CustomerDetailId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("InvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IssueDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Operation")
                        .HasColumnType("int");

                    b.Property<int?>("PaymentType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerDetailId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("CashProceeds", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.CashRegister", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("BalanceAmount")
                        .HasColumnType("money");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CurrencyType")
                        .HasColumnType("int");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("money");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("WithdrawalAmount")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.ToTable("CashRegister", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("46faf77b-ac26-4977-ad4c-31e97ebded23"),
                            BalanceAmount = 0m,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CurrencyType = 1,
                            DepositAmount = 0m,
                            Name = "TL Kasası",
                            WithdrawalAmount = 0m
                        });
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.CashRegisterDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CashRegisterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("money");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Opposite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Processor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("WithdrawalAmount")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CashRegisterId");

                    b.ToTable("CashRegisterDetail", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsParent")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Debit")
                        .HasColumnType("money");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("money");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxDepartment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Town")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Withdrawal")
                        .HasColumnType("money");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.CustomerDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CashRegisterDetailId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("money");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DueDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssueDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Operation")
                        .HasColumnType("int");

                    b.Property<string>("Opposite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Payment")
                        .HasColumnType("int");

                    b.Property<string>("Processor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("money");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("WithdrawalAmount")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("CustomerDetail", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CurrencyType")
                        .HasColumnType("int");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DepositAmount")
                        .HasColumnType("money");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DueDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssueDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Operation")
                        .HasColumnType("int");

                    b.Property<int?>("Payment")
                        .HasColumnType("int");

                    b.Property<string>("Processor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("money");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("WithdrawalAmount")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Invoice", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("decimal(7,2)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Withdrawal")
                        .HasColumnType("decimal(7,2)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.ProductDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CustomerDetailId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("InvoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Processor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerDetailId");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductDetail", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.BankDetail", b =>
                {
                    b.HasOne("Domain.Entities.CompanyEntities.Bank", "Bank")
                        .WithMany("Details")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.CashProceed", b =>
                {
                    b.HasOne("Domain.Entities.CompanyEntities.CustomerDetail", "CustomerDetail")
                        .WithMany("CashProceeds")
                        .HasForeignKey("CustomerDetailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.CompanyEntities.Invoice", "Invoice")
                        .WithMany("CashProceeds")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("Domain.Entities.CompanyEntities.Cheque", "Cheque", b1 =>
                        {
                            b1.Property<Guid>("CashProceedId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("BankName")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("BankName");

                            b1.Property<string>("ChequeNumber")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("ChequeNumber");

                            b1.Property<string>("MaturityDate")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CashProceedId");

                            b1.ToTable("CashProceeds");

                            b1.WithOwner()
                                .HasForeignKey("CashProceedId");
                        });

                    b.Navigation("Cheque");

                    b.Navigation("CustomerDetail");

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.CashRegisterDetail", b =>
                {
                    b.HasOne("Domain.Entities.CompanyEntities.CashRegister", "CashRegister")
                        .WithMany("Details")
                        .HasForeignKey("CashRegisterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CashRegister");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Category", b =>
                {
                    b.HasOne("Domain.Entities.CompanyEntities.Category", "ParentCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.CustomerDetail", b =>
                {
                    b.HasOne("Domain.Entities.CompanyEntities.Customer", "Customer")
                        .WithMany("Details")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Invoice", b =>
                {
                    b.HasOne("Domain.Entities.CompanyEntities.Customer", "Customer")
                        .WithMany("Invoices")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Product", b =>
                {
                    b.HasOne("Domain.Entities.CompanyEntities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("Domain.Entities.CompanyEntities.Products.Attributes", "Attributes", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("AdditionalAttributes")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Brand")
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Dimensions")
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Weight")
                                .HasColumnType("nvarchar(50)");

                            b1.HasKey("ProductId");

                            b1.ToTable("Product");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("Domain.Entities.CompanyEntities.Products.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Barcode")
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Image")
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("SKU")
                                .HasColumnType("nvarchar(50)");

                            b1.Property<string>("Supplier")
                                .HasColumnType("nvarchar(50)");

                            b1.HasKey("ProductId");

                            b1.ToTable("Product");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.OwnsOne("Domain.Entities.CompanyEntities.Products.Stock", "Stock", b1 =>
                        {
                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<decimal>("StockQuantity")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<int?>("UnitOfMeasure")
                                .HasColumnType("int");

                            b1.HasKey("ProductId");

                            b1.ToTable("Product");

                            b1.WithOwner()
                                .HasForeignKey("ProductId");
                        });

                    b.Navigation("Attributes");

                    b.Navigation("Category");

                    b.Navigation("Metadata");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.ProductDetail", b =>
                {
                    b.HasOne("Domain.Entities.CompanyEntities.CustomerDetail", "CustomerDetail")
                        .WithMany("Products")
                        .HasForeignKey("CustomerDetailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.CompanyEntities.Invoice", "Invoice")
                        .WithMany("Products")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Entities.CompanyEntities.Product", "Product")
                        .WithMany("Operations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Domain.Entities.CompanyEntities.Products.Pricing", "Pricing", b1 =>
                        {
                            b1.Property<Guid>("ProductDetailId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("DiscountRate")
                                .IsRequired()
                                .HasColumnType("nvarchar(50)");

                            b1.Property<decimal>("Quantity")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<decimal>("TaxRate")
                                .HasColumnType("money");

                            b1.Property<decimal>("TotalPrice")
                                .HasColumnType("money");

                            b1.Property<decimal>("UnitPrice")
                                .HasColumnType("money");

                            b1.HasKey("ProductDetailId");

                            b1.ToTable("ProductDetail");

                            b1.WithOwner()
                                .HasForeignKey("ProductDetailId");
                        });

                    b.Navigation("CustomerDetail");

                    b.Navigation("Invoice");

                    b.Navigation("Pricing");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Bank", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.CashRegister", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Category", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Customer", b =>
                {
                    b.Navigation("Details");

                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.CustomerDetail", b =>
                {
                    b.Navigation("CashProceeds");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Invoice", b =>
                {
                    b.Navigation("CashProceeds");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Domain.Entities.CompanyEntities.Product", b =>
                {
                    b.Navigation("Operations");
                });
#pragma warning restore 612, 618
        }
    }
}
