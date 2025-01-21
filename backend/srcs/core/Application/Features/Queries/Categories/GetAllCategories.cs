using Application.Features.Queries.CashRegisterDetails;
using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Application.Features.Queries.Categories;

public sealed record GetAllCategories() : IRequest<Result<List<Category>>> {
	public int PageNumber { get; set; } = 0;
	public int PageSize   { get; set; } = 10;
}




internal sealed record GetAllCategoriesHandler(
	ICategoryRepository categoryRepository) : IRequestHandler<GetAllCategories, Result<List<Category>>> {
	public async Task<Result<List<Category>>> Handle(GetAllCategories request, CancellationToken cancellationToken) {
		int pageNumber = request.PageNumber;
		int pageSize   = request.PageSize;

		List<Category> categories = await categoryRepository.GetAll()
															.OrderBy(c => c.Name)
															.Skip(pageNumber * pageSize)
															.Take(pageSize)
															.Include(c => c.SubCategories)
															.Include(c => c.Products)
															.ToListAsync(cancellationToken);

		categories.ForEach(c => c.SubCategories?.ForEach(sc => {
			sc.SubCategories = null;
			sc.ParentCategory = null;
		}));

		return categories;
	}
}