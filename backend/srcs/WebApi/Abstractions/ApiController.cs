using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Abstractions;


[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public abstract class ApiController : ControllerBase {
	protected readonly IMediator Mediator;
	
	protected ApiController(IMediator mediator) {
		Mediator = mediator;
	}
}