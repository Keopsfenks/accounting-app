using Application.Features.Commands.BankDetails.BankDetailCreate;
using Application.Features.Commands.BankDetails.BankDetailDelete;
using Application.Features.Commands.BankDetails.BankDetailUpdate;
using Application.Features.Queries.BankDetails;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class BankDetailController(IMediator mediator) : ApiController(mediator) {

	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateBankDetail(BankDetailCreateRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateBankDetail(BankDetailUpdateRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> BankDetailDelete(BankDetailDeleteRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllBankDetails([FromQuery] GetAllBankDetails request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}