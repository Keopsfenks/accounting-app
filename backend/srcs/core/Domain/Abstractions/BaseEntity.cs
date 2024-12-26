namespace Domain.Abstractions;

public abstract class BaseEntity {
	public Guid   Id        { get; set; }

	protected BaseEntity() {
		Id = Guid.NewGuid();
	}

	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}