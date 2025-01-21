using System.Security.Claims;
using Persistance.Services;
using Persistance.Services.Interface;

namespace WebApi.Services;

public sealed class CurrentContextService(IHttpContextAccessor httpContextAccessor) : ICurrentContextService {

	public Dictionary<string, string> Claims => httpContextAccessor.HttpContext?.User.Claims
																	.ToDictionary(c => c.Type, c => c.Value) ?? new Dictionary<string, string>();
	public Dictionary<string, string> Headers => httpContextAccessor.HttpContext?.Request.Headers
																	.ToDictionary(h => h.Key, h => h.Value.ToString()) ?? new Dictionary<string, string>();
}