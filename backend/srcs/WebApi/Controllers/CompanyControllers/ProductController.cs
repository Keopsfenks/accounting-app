using Application.Features.Commands.Products.CreateProduct;
using Application.Features.Commands.Products.DeleteProduct;
using Application.Features.Commands.Products.UpdateProduct;
using Application.Features.Queries.Products;
using Infrastructure.Companies.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;

namespace WebApi.Controllers.CompanyControllers;

public sealed class ProductController(IMediator mediator) : ApiController(mediator) {
	[HttpPost]
	[CompanyAuthorization]
	public async Task<IActionResult> CreateProduct(CreateProductRequest createRequest) {
		var response = await Mediator.Send(createRequest);
		return Ok(response);
	}
	[HttpDelete]
	[CompanyAuthorization]
	public async Task<IActionResult> DeleteProduct(DeleteProductRequest deleteRequest) {
		var response = await Mediator.Send(deleteRequest);
		return Ok(response);
	}
	[HttpPut]
	[CompanyAuthorization]
	public async Task<IActionResult> UpdateProduct(UpdateProductRequest updateRequest) {
		var response = await Mediator.Send(updateRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProducts getAllRequest) {
		var response = await Mediator.Send(getAllRequest);
		return Ok(response);
	}
	[HttpGet]
	[CompanyAuthorization]
	public async Task<IActionResult> GetAllProductDetails([FromQuery] GetAllProductsDetail getAllRequest) {
		var response = await Mediator.Send(getAllRequest);
		return Ok(response);
	}
}