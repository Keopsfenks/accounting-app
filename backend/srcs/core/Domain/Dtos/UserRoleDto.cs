namespace Domain.Dtos;

public sealed record UserRoleDto() {
	public Guid   UserId      { get; set; }
	public Guid   RoleId      { get; set; }
	public Guid   CompanyId   { get; set; }
	public string UserName    { get; set; }
	public string CompanyName { get; set; }
	public string RoleName    { get; set; }
}
