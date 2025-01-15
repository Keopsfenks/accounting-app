using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Categories.DeleteCategory;

internal sealed record DeleteCategoryHandler(
	ICategoryRepository categoryRepository,
	IUnitOfWorkCompany  unitOfWorkCompany) : IRequestHandler<DeleteCategoryRequest, Result<string>> {

	public async Task<Result<string>> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken) {
		Category? category = await categoryRepository.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

		if (category is null)
			return (500, "Category not found");

		if (category.Products.Count > 0)
			return (500, "Category has products, cannot delete");
		if (category.SubCategories.Count > 0)
			return (500, "Category has sub categories, cannot delete");

		categoryRepository.Delete(category);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

		return "Category deleted successfully";
	}
}