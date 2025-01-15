using Application.Features.Commands.Authentication;
using MediatR;
using TS.Result;

namespace Application.Features.Commands.Authentications.ChangeCompany;

public sealed record ChangeCompanyRequest(Guid? CompanyId) : IRequest<Result<LoginResponse>>;