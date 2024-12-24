using Domain.Abstractions;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Company : BaseEntity {
	public string   Name          { get; set; } = string.Empty;
	public string   TaxId         { get; set; } = string.Empty;
	public string   TaxDepartment { get; set; } = string.Empty;
	public string   Address       { get; set; } = string.Empty;
	public Database Database      { get; set; } = new(string.Empty, string.Empty, string.Empty, string.Empty);

	public ICollection<CompanyUsers> UserRoles { get; set; } = new List<CompanyUsers>();
	// Navigation Property
}