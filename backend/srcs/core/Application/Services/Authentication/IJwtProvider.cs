using Application.Features.Commands.Authentication;
using Domain.Entities;

namespace Application.Services.Authentication;

public interface IJwtProvider {
	Task<LoginResponse> GenerateJwtToken(AppUser user, Guid? companyId, List<Company> companies);}