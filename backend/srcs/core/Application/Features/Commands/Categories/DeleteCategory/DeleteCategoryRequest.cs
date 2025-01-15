using MediatR;
using TS.Result;

namespace Application.Features.Commands.Categories.DeleteCategory;

public sealed record DeleteCategoryRequest(
	Guid Id) : IRequest<Result<string>>;