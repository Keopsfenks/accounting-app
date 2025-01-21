using Application.Features.Commands.Categories.CreateCategory;
using Application.Features.Commands.Categories.DeleteCategory;
using Application.Features.Commands.Categories.UpdateCategory;
using Application.Features.Queries.Categories;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class CategoryController(IMediator mediator) : ApiController(mediator) {
	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateCategory(CreateCategoryRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> DeleteCategory(DeleteCategoryRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllCategorys([FromQuery] GetAllCategories getAllRequest) {
		var response = await Mediator.Send(getAllRequest);
		return Ok(response);
	}
}