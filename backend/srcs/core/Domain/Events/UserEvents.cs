using MediatR;

namespace Domain.Events;

public sealed class UserEvents : INotification {
	public Guid Id { get; private set; }

	public UserEvents(Guid ıd) {
		Id = ıd;
	}
}