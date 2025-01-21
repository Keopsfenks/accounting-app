using Application.Features.Commands.CashRegisterDetails.CashRegisterDetailCreate;
using Application.Features.Commands.CashRegisterDetails.CashRegisterDetailDelete;
using Application.Features.Commands.CashRegisterDetails.CashRegisterDetailUpdate;
using Application.Features.Queries.CashRegisterDetails;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class CashRegisterDetailController(IMediator mediator) : ApiController(mediator) {
	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateCashRegisterDetail(CashRegisterDetailCreateRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> DeleteCashRegisterDetail(CashRegisterDetailDeleteRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateCashRegisterDetail(CashRegisterDetailUpdateRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllCashRegisterDetail([FromQuery] GetAllCashRegisterDetail getAllRequest) {
		var response = await Mediator.Send(getAllRequest);
		return Ok(response);
	}
}