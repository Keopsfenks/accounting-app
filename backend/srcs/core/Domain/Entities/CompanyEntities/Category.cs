using System.Text.Json.Serialization;
using Domain.Abstractions;

namespace Domain.Entities.CompanyEntities;

public sealed class Category : BaseEntity {
	public string  Name        { get; set; }
	public string? Description { get; set; }
	public bool    IsActive    { get; set; } = true;

	public Guid?     ParentCategoryId { get; set; }
	public bool      IsParent         { get; set; } = false;
	public Category? ParentCategory   { get; set; }

	public List<Category> SubCategories { get; set; } = new();
	public List<Product>  Products      { get; set; } = new();
}