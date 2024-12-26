using Domain.ValueObjects;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Companies.CreateCompany;

public sealed record CreateCompanyRequest(
	string Name,
	string Address,
	string   TaxDepartment,
	string   TaxId,
	Database Database) : IRequest<Result<string>>;