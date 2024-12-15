namespace Application.Features.Commands.Authentication;

public sealed record LoginResponse(
	string   Token,
	string   RefreshToken,
	DateTime RefreshTokenExpires);