using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Application.Features.Commands.Authentication;
using Application.Services.Authentication;
using Domain.Entities;
using Infrastructure.Authentication.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication.Services;

internal class JwtProvider(
	UserManager<AppUser> userManager,
	IOptions<JwtOptions> jwtOptions) : IJwtProvider {
	public async Task<LoginResponse> GenerateJwtToken(AppUser user, Guid? companyId, List<Company> companies) {
		List<Claim> claims = new() {
			new Claim("Id",        user.Id.ToString()),
			new Claim("Fullname",  user.FullName),
			new Claim("Email",     user.Email           ??string.Empty),
			new Claim("Username",  user.UserName        ??string.Empty),
			new Claim("CompanyId", companyId.ToString() ?? string.Empty),
			new Claim("Companies", JsonSerializer.Serialize(companies))
		};
		
		DateTime expires = DateTime.UtcNow.AddMonths(1);
		
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey));
		JwtSecurityToken jwtSecurityToken = new(
			issuer: jwtOptions.Value.Issuer,
			audience: jwtOptions.Value.Audience,
			claims: claims,
			notBefore: DateTime.UtcNow,
			expires: expires,
			signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
		);
		JwtSecurityTokenHandler tokenHandler = new();
		
		string token = tokenHandler.WriteToken(jwtSecurityToken);
		
		string   refreshToken        = Guid.NewGuid().ToString();
		DateTime refreshTokenExpires = DateTime.UtcNow.AddHours(1);
		
		user.RefreshToken = refreshToken;
		user.RefreshTokenExpires = refreshTokenExpires;

		await userManager.UpdateAsync(user);

		return new(token, refreshToken, refreshTokenExpires);
	}

	public Task<LoginResponse> CreateToken(AppUser user, Guid? companyId, List<Company> companies) {
		throw new NotImplementedException();
	}
}