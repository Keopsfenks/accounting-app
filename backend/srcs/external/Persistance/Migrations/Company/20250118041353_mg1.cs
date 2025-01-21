using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations.Company
{
    /// <inheritdoc />
    public partial class mg1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CashRegister",
                keyColumn: "Id",
                keyValue: new Guid("46faf77b-ac26-4977-ad4c-31e97ebded23"));

            migrationBuilder.DropColumn(
                name: "Attributes_AdditionalAttributes",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Attributes_Brand",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Attributes_Dimensions",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Attributes_Weight",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Metadata_Barcode",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Metadata_Image",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Metadata_SKU",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Metadata_Supplier",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Stock_StockQuantity",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Withdrawal",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "Stock_UnitOfMeasure",
                table: "Product",
                newName: "UnitOfMeasure");

            migrationBuilder.InsertData(
                table: "CashRegister",
                columns: new[] { "Id", "BalanceAmount", "CreatedAt", "CurrencyType", "DepositAmount", "Name", "UpdatedAt", "WithdrawalAmount" },
                values: new object[] { new Guid("d610fcfd-71f6-4143-963b-c12e968d5312"), 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0m, "TL Kasası", null, 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CashRegister",
                keyColumn: "Id",
                keyValue: new Guid("d610fcfd-71f6-4143-963b-c12e968d5312"));

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "Product",
                newName: "Stock_UnitOfMeasure");

            migrationBuilder.AddColumn<string>(
                name: "Attributes_AdditionalAttributes",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attributes_Brand",
                table: "Product",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attributes_Dimensions",
                table: "Product",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attributes_Weight",
                table: "Product",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "Product",
                type: "decimal(7,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Barcode",
                table: "Product",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Image",
                table: "Product",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata_SKU",
                table: "Product",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Supplier",
                table: "Product",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Stock_StockQuantity",
                table: "Product",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Withdrawal",
                table: "Product",
                type: "decimal(7,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "CashRegister",
                columns: new[] { "Id", "BalanceAmount", "CreatedAt", "CurrencyType", "DepositAmount", "Name", "UpdatedAt", "WithdrawalAmount" },
                values: new object[] { new Guid("46faf77b-ac26-4977-ad4c-31e97ebded23"), 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0m, "TL Kasası", null, 0m });
        }
    }
}
