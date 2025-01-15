using Application.Features.Commands.Companies.CreateCompany;
using Application.Features.Commands.Companies.DeleteCompany;
using Application.Features.Commands.Companies.UpdateCompany;
using Application.Features.Commands.Companies.UpdateDatabase;
using Application.Features.Queries.Companies;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers;

public sealed class CompanyController(IMediator mediator) : ApiController(mediator) {
	[HttpPost]
	public async Task<IActionResult> CreateCompany(CreateCompanyRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpDelete]
	//[CompanyAuthorization("Admin")]
	public async Task<IActionResult> DeleteCompany(DeleteCompanyRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpPut]
	//[CompanyAuthorization("Admin")]
	public async Task<IActionResult> UpdateCompany(UpdateCompanyRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpGet]
	public async Task<IActionResult> GetAllCompanies([FromQuery] GetAllCompanies request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpGet]
	public async Task<IActionResult> GetAllUserToCompanies([FromQuery] GetAllUserToCompanies request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}

	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateCompanyDatabase(UpdateCompanyDatabase request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
}