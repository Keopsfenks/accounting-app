using MediatR;
using TS.Result;

namespace Application.Features.Commands.Customers.CreateCustomer;

public sealed record CreateCustomerRequest(
	string  Name,
	int     CustomerType,
	string? Description,
	string? Email,
	string? Phone,
	string? Address,
	string? City,
	string? Town,
	string? Country,
	string? ZipCode,
	string? TaxId,
	string? TaxDepartment) : IRequest<Result<string>>;