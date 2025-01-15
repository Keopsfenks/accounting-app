using Application.Features.Commands.Invoices.CreateInvoice;
using Application.Features.Commands.Invoices.DeleteInvoice;
using Application.Features.Commands.Invoices.UpdateInvoice;
using Application.Features.Queries.Invoices;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class InvoiceController(IMediator mediator) : ApiController(mediator) {
	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateInvoice(CreateInvoiceRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> DeleteInvoice(DeleteInvoiceRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateInvoice(UpdateInvoiceRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllInvoices([FromQuery] GetAllInvoices getAllRequest) {
		var response = await Mediator.Send(getAllRequest);
		return Ok(response);
	}
}