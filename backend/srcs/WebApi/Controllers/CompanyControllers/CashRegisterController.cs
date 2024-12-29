using Application.Features.CompanyFeatures.CashRegisterCreate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class CashRegisterController(IMediator mediator) : ApiController(mediator) {
	[HttpPost]
	public async Task<IActionResult> CreateCashRegister(CashRegisterRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}