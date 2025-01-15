using Application.Features.Commands.Categories.CreateCategory;
using Application.Features.Commands.Categories.DeleteCategory;
using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Categories.UpdateCategory;

public sealed record UpdateCategoryRequest(
	Guid    Id,
	string  Name,
	string? Description,
	Guid?   parentCategory) : IRequest<Result<string>>;


internal sealed record UpdateCategoryHandler(
	ICategoryRepository categoryRepository,
	IMediator mediator) : IRequestHandler<UpdateCategoryRequest, Result<string>> {
	public async Task<Result<string>> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken) {

		Result<string> deleteResult = await mediator.Send(new DeleteCategoryRequest(request.Id), cancellationToken);

		if (deleteResult.IsSuccessful is false)
			return deleteResult;

		Result<string> createResult = await mediator.Send(new CreateCategoryRequest(request.Name, request.Description, request.parentCategory), cancellationToken);

		if (createResult.IsSuccessful is false)
			return createResult;

		return "Category updated successfully";
	}
}