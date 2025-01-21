using System.Text.Json.Serialization;
using Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class CompanyUsers : BaseEntity {
	public Guid    CompanyId { get; set; }
	 [JsonIgnore]
	public Company Company   { get; set; } = null!;

	public Guid    UserId { get; set; }
	[JsonIgnore]
	public AppUser User   { get; set; } = null!;

	public Guid    RoleId { get; set; }
	public string  RoleName { get; set; } = string.Empty;
}