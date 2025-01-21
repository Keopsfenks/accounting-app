using Application.Features.Commands.Banks.BankCreate;
using Application.Features.Commands.Banks.BankDelete;
using Application.Features.Commands.Banks.BankUpdate;
using Application.Features.Queries.Banks;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class BankController(IMediator mediator) : ApiController(mediator) {

	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateBank(BankCreateRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateBank(BankUpdateRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> BankDelete(BankDeleteRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllBanks([FromQuery] GetAllBanks request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}