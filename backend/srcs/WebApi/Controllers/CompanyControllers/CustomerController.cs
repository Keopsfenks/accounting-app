/*using Application.Features.Commands.Customers.CreateCustomer;
using Application.Features.Commands.Customers.DeleteCustomer;
using Application.Features.Commands.Customers.UpdateCustomer;
using Application.Features.Queries.Customers;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class CustomerController(IMediator mediator) : ApiController(mediator) {

	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateCustomer(CreateCustomerRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> DeleteCustomer(DeleteCustomerRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateCustomer(UpdateCustomerRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllCustomers([FromQuery] GetAllCustomer getAllRequest) {
		var response = await Mediator.Send(getAllRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetIdToCustomer([FromQuery] GetIdToCustomer getAllRequest) {
		var response = await Mediator.Send(getAllRequest);
		return Ok(response);
	}
}*/