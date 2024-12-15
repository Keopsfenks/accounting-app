using Application.Features.Commands.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers;

public sealed class AuthController : ApiController {
	public AuthController(IMediator mediator) : base(mediator) { }
	
	[HttpPost]
	public async Task<IActionResult> Login(LoginRequest request) {
		var response = await Mediator.Send(request);
		Console.WriteLine("Mediator", Mediator);
		return Ok(response);
	}
}