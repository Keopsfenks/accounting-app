using Application.Features.Commands.CustomerDetails.CreateCustomeDetails;
using Application.Features.Commands.CustomerDetails.DeleteCustomerDetails;
using Application.Features.Commands.CustomerDetails.UpdateCustomerDetails;
using Application.Features.Queries.CustomerDetails;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class CustomerDetailController(IMediator mediator) : ApiController(mediator) {

	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateCustomerDetail(CreateCustomerDetailRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> DeleteCustomerDetail(DeleteCustomerDetailRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateCustomerDetail(UpdateCustomerDetailRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllCustomerDetails([FromQuery] GetAllCustomerDetail getAllRequest) {
		var response = await Mediator.Send(getAllRequest);
		return Ok(response);
	}
}