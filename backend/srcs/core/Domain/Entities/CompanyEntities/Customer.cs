using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities.CompanyEntities;

public sealed class Customer : BaseEntity {
	public string           Name        { get; set; }
	public CustomerTypeEnum Type        { get; set; } = CustomerTypeEnum.Individual;
	public string?          Description { get; set; }

	public string?              Email         { get; set; }
	public string?              Phone         { get; set; }
	public string?              Address       { get; set; }
	public string?              City          { get; set; }
	public string?              Town          { get; set; }
	public string?              Country       { get; set; }
	public string?              ZipCode       { get; set; }
	public string?              TaxId         { get; set; }
	public string?              TaxDepartment { get; set; }
	public List<CustomerDetail> Details       { get; set; }
	public List<Invoice>        Invoices      { get; set; }
	public decimal Deposit    { get; set; } = 0;
	public decimal Withdrawal { get; set; } = 0;
	public decimal Debit    { get; set; } = 0;
}