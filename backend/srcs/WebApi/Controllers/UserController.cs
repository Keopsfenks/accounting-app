using Application.Features.Commands.Users.DeleteUser;
using Application.Features.Commands.Users.UpdateUser;
using Application.Features.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers;

public sealed class UserController : ApiController {
	public UserController(IMediator mediator) : base(mediator) { }
	
	[HttpGet]
	public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsers request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	
	[HttpPost]
	public async Task<IActionResult> UpdateUser(UpdateRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}