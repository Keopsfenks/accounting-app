﻿using Application.Features.Commands.Companies.CreateCompany;
using Application.Features.Commands.Companies.DeleteCompany;
using Application.Features.Commands.Companies.UpdateCompany;
using Application.Features.Queries.Companies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers;

[Authorize]
public sealed class CompanyController : ApiController{
	public CompanyController(IMediator mediator) : base(mediator) {
	}

	[HttpPost]
	public async Task<IActionResult> CreateCompany(CreateCompanyRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpDelete]
	public async Task<IActionResult> DeleteCompany(DeleteCompanyRequest request) {
		var response = await Mediator.Send(request);
		return Ok(response);
	}
	[HttpPut]
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
}