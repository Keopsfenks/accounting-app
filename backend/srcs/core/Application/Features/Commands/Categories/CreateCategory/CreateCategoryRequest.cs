using AutoMapper;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Categories.CreateCategory;

public sealed record CreateCategoryRequest(
	string Name,
	string? Description,
	Guid? parentCategory) : IRequest<Result<string>>;