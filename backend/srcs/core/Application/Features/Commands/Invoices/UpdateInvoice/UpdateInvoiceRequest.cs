using MediatR;
using TS.Result;

namespace Application.Features.Commands.Invoices.UpdateInvoice;

public sealed record UpdateInvoiceRequest(
	Guid Id) : IRequest<Result<string>>;

internal sealed record UpdateInvoiceHandler() : IRequestHandler<UpdateInvoiceRequest, Result<string>> {
	public async Task<Result<string>> Handle(UpdateInvoiceRequest request, CancellationToken cancellationToken) {
		return "Invoice Successfully Updated";
	}
}