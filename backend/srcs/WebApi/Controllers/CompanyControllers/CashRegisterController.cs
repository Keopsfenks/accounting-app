using Application.Features.Commands.CashRegisterDetails.CashRegisterDetailCreate;
using Application.Features.Commands.CashRegisters.CashRegisterCreate;
using Application.Features.Commands.CashRegisters.CashRegisterDelete;
using Application.Features.Commands.CashRegisters.CashRegisterUpdate;
using Application.Features.Queries.CashRegisters;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class CashRegisterController(IMediator mediator) : ApiController(mediator) {
	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateCashRegister(CashRegisterCreateRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateCashRegister(CashRegisterUpdateRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> DeleteCashRegister(CashRegisterDeleteRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}

	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllCashRegister([FromQuery] GetAllCashRegister request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}