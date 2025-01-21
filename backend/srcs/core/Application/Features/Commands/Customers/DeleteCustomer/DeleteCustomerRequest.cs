using MediatR;
using TS.Result;

namespace Application.Features.Commands.Customers.DeleteCustomer;

public sealed record DeleteCustomerRequest(
	Guid Id) : IRequest<Result<string>>;