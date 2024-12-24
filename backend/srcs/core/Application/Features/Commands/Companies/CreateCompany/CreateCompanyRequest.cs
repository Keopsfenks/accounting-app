using Domain.ValueObjects;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Companies.CreateCompany;

public sealed record CreateCompanyRequest(
	string   Name,
	string   FullAddress,
	string   TaxDepartment,
	string   TaxId,
	Guid   UserId,
	Database Database) : IRequest<Result<string>>;