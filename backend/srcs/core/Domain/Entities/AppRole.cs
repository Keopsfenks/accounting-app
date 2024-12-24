using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class AppRole : IdentityRole<Guid> {
	public bool      IsDeleted           { get; set; } = false;
}