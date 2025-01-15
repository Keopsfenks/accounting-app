using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Categories.CreateCategory;

internal sealed record CreateCategoryHandler(
	ICategoryRepository categoryRepositories,
	IUnitOfWorkCompany  unitOfWorkCompany) : IRequestHandler<CreateCategoryRequest, Result<string>> {
	public async Task<Result<string>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken) {
		Category? category = await categoryRepositories.FirstOrDefaultAsync(c => c.Name == request.Name,
																			cancellationToken);
		Category? parentCategory = await categoryRepositories.FirstOrDefaultAsync(c => c.Id == request.parentCategory,
			cancellationToken);

		if (category is not null)
			return (500, "Category already exist");

		Category newCategory = new() {
										 Name             = request.Name,
										 Description      = request.Description,
										 ParentCategoryId = request.parentCategory,
										 ParentCategory   = parentCategory,
										 IsParent         = request.parentCategory is not null,
									 };

		await categoryRepositories.AddAsync(newCategory, cancellationToken);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return "Category created successfully";
	}
}