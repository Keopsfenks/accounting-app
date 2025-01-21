using Application.Features.Commands.CashProceeds.CreateCashProceeds;
using Application.Features.Commands.CashProceeds.DeleteCashProceeds;
using Application.Features.Commands.CashProceeds.UpdateCashProceeds;
using Application.Features.Queries.CashProceeds;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class CashProceedController(IMediator mediator) : ApiController(mediator) {
	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateCashProceeds(CreateCashProceedsRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateCashProceeds(UpdateCashProceedsRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> CashProceedsDelete(DeleteCashProceedsRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllCashProceeds([FromQuery] GetAllCashProceeds request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}