using Domain.Entities;
using Domain.Repositories;
using FluentEmail.Core;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Domain.Events;

public sealed class UserEvents : INotification {
	public Guid   Id { get; private set; }

	public UserEvents(Guid ıd) {
		Id = ıd;
	}
}

public sealed class UserEventsHandler(
	UserManager<AppUser>   userManager,
	ICompanyUserRepository companyUserRepository,
	IUnitOfWork            unitOfWork,
	IFluentEmail           fluentEmail) : INotificationHandler<UserEvents> {
	public async Task Handle(UserEvents notification, CancellationToken cancellationToken) {
		AppUser? user = await userManager.FindByIdAsync(notification.Id.ToString());

		if (user is not null)
			await fluentEmail
				 .To(user.Email)
				 .Subject("Mail Confirmation")
				 .Body(CreateBody(user), true)
				 .SendAsync(cancellationToken);
	}

	private string CreateBody(AppUser user) {
		string body = $@"
		Mail adresinizi onaylamak için aşağıdaki linkle tıklayın. 
		<a href='http://localhost:4200/confirm-email/{user.Email}' target='_blank'>Maili Onaylamak için tıklayın
		</a>
		";
		return body;
	}
}