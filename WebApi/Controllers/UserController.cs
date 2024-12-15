using Application.Features.Commands.Users.CreateUser;
using Application.Features.Commands.Users.DeleteUser;
using Application.Features.Commands.Users.UpdateUser;
using Application.Features.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using RegisterRequest = Application.Features.Commands.Users.CreateUser.RegisterRequest;

namespace WebApi.Controllers;

public sealed class UserController : ApiController {
	public UserController(IMediator mediator) : base(mediator) { }
	
	[HttpPost]
	public async Task<IActionResult> Register(RegisterRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	
	[HttpPost]
	public async Task<IActionResult> GetAllUsers(GetAllUsers request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	
	[HttpDelete]
	public async Task<IActionResult> DeleteUser(DeleteRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	
	[HttpPost]
	public async Task<IActionResult> UpdateUser(UpdateRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}