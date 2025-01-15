using Domain.Entities.CompanyEntities;
using Domain.Repositories.CompanyRepositories;
using MediatR;
using Persistance.Services;
using TS.Result;

namespace Application.Features.Commands.Products.CreateProduct;

internal sealed record CreateProductHandler(
	ICategoryRepository categoryRepository,
	IProductRepository  productRepository,
	IUnitOfWorkCompany  unitOfWorkCompany) : IRequestHandler<CreateProductRequest, Result<string>> {
	public async Task<Result<string>> Handle(CreateProductRequest request, CancellationToken cancellationToken) {
		Category? category = await categoryRepository.FirstOrDefaultAsync(c => c.Id == request.Category);

		if (category is null)
			return (500, "Category not found");

		Product? product = new Product {
										   Name        = request.Name,
										   Description = request.Description,
										   IsActive    = bool.Parse(request.IsActive),
										   CategoryId  = request.Category,
										   Category    = category,
										   Attributes  = request.Attributes,
										   Metadata    = request.Metadata,
										   Stock       = request.Stock,
									   };
		
		await productRepository.AddAsync(product, cancellationToken);
		await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
		
		return "Product created successfully";
	}
}