using Application.Features.Commands.Authentication;
using Application.Features.Commands.Users.CreateUser;
using Application.Features.Commands.Users.DeleteUser;
using Application.Features.Commands.Users.EmailConfirmation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers;
[AllowAnonymous]
public sealed class AuthController : ApiController {
	public AuthController(IMediator mediator) : base(mediator) { }
	
	[HttpPost]
	public async Task<IActionResult> Login(LoginRequest request) {
		var response = await Mediator.Send(request);
		Console.WriteLine("Mediator", Mediator);
		return Ok(response);
	}
	[HttpPost]
	public async Task<IActionResult> ConfirmEmail(EmailConfirmationRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	
	[HttpPost]
	public async Task<IActionResult> Register(RegisterRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	
	[HttpDelete]
	public async Task<IActionResult> DeleteUser(DeleteRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}