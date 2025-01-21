using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations.Company
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyType = table.Column<int>(type: "int", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "money", nullable: false),
                    WithdrawAmount = table.Column<decimal>(type: "money", nullable: false),
                    Balance = table.Column<decimal>(type: "money", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashRegister",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyType = table.Column<int>(type: "int", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "money", nullable: false),
                    WithdrawalAmount = table.Column<decimal>(type: "money", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "money", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRegister", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsParent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Town = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deposit = table.Column<decimal>(type: "money", nullable: false),
                    Withdrawal = table.Column<decimal>(type: "money", nullable: false),
                    Debit = table.Column<decimal>(type: "money", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "money", nullable: false),
                    WithdrawalAmount = table.Column<decimal>(type: "money", nullable: false),
                    Opposite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDetail_Bank_BankId",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashRegisterDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "money", nullable: false),
                    WithdrawalAmount = table.Column<decimal>(type: "money", nullable: false),
                    Opposite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashRegisterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRegisterDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashRegisterDetail_CashRegister_CashRegisterId",
                        column: x => x.CashRegisterId,
                        principalTable: "CashRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Deposit = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    Withdrawal = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Attributes_Brand = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Attributes_Dimensions = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Attributes_Weight = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Attributes_AdditionalAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metadata_Barcode = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Metadata_Image = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Metadata_Supplier = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Metadata_SKU = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Stock_StockQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Stock_UnitOfMeasure = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opposite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "money", nullable: false),
                    WithdrawalAmount = table.Column<decimal>(type: "money", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "money", nullable: false),
                    Payment = table.Column<int>(type: "int", nullable: true),
                    Operation = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CashRegisterDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDetail_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepositAmount = table.Column<decimal>(type: "money", nullable: false),
                    WithdrawalAmount = table.Column<decimal>(type: "money", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "money", nullable: false),
                    CurrencyType = table.Column<int>(type: "int", nullable: true),
                    Payment = table.Column<int>(type: "int", nullable: true),
                    Operation = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CashProceeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: true),
                    Operation = table.Column<int>(type: "int", nullable: true),
                    Cheque_MaturityDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashProceeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashProceeds_CustomerDetail_CustomerDetailId",
                        column: x => x.CustomerDetailId,
                        principalTable: "CustomerDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CashProceeds_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Pricing_Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Pricing_UnitPrice = table.Column<decimal>(type: "money", nullable: true),
                    Pricing_TaxRate = table.Column<decimal>(type: "money", nullable: true),
                    Pricing_TotalPrice = table.Column<decimal>(type: "money", nullable: true),
                    Pricing_DiscountRate = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetail_CustomerDetail_CustomerDetailId",
                        column: x => x.CustomerDetailId,
                        principalTable: "CustomerDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CashRegister",
                columns: new[] { "Id", "BalanceAmount", "CreatedAt", "CurrencyType", "DepositAmount", "Name", "UpdatedAt", "WithdrawalAmount" },
                values: new object[] { new Guid("46faf77b-ac26-4977-ad4c-31e97ebded23"), 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0m, "TL Kasası", null, 0m });

            migrationBuilder.CreateIndex(
                name: "IX_BankDetail_BankId",
                table: "BankDetail",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_CashProceeds_CustomerDetailId",
                table: "CashProceeds",
                column: "CustomerDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CashProceeds_InvoiceId",
                table: "CashProceeds",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CashRegisterDetail_CashRegisterId",
                table: "CashRegisterDetail",
                column: "CashRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                table: "Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDetail_CustomerId",
                table: "CustomerDetail",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CustomerId",
                table: "Invoice",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_CustomerDetailId",
                table: "ProductDetail",
                column: "CustomerDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_InvoiceId",
                table: "ProductDetail",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetail_ProductId",
                table: "ProductDetail",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankDetail");

            migrationBuilder.DropTable(
                name: "CashProceeds");

            migrationBuilder.DropTable(
                name: "CashRegisterDetail");

            migrationBuilder.DropTable(
                name: "ProductDetail");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "CashRegister");

            migrationBuilder.DropTable(
                name: "CustomerDetail");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
