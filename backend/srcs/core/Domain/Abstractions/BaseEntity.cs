namespace Domain.Abstractions;

public abstract class BaseEntity {
	public Guid   Id        { get; set; }

	protected BaseEntity() {
		Id = Guid.NewGuid();
	}

	public bool IsDeleted { get; set; } = false;
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}