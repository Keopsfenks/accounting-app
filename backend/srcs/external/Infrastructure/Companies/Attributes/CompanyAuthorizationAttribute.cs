using System.Text.Json;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Persistance.Services.Interface;

namespace Infrastructure.Companies.Attributes;

public sealed class CompanyAuthorizationAttribute : TypeFilterAttribute {
	public CompanyAuthorizationAttribute(string roles = "")
		: base(typeof(CompanyAuthorizationFilter)) {
		Arguments = new object[] { roles };
	}
}

public sealed class CompanyAuthorizationFilter : IAsyncAuthorizationFilter {
	private readonly ICompanyUserRepository _companyUserRepository;
	private readonly string[]               _roles;
	private readonly RoleManager<AppRole>	_roleManager;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CompanyAuthorizationFilter(
		ICompanyUserRepository companyUserRepository,
		string                 roles, RoleManager<AppRole> roleManager,
		IHttpContextAccessor currentContextService) {
		_companyUserRepository = companyUserRepository;
		_httpContextAccessor = currentContextService;
		_roleManager                = roleManager;
		_roles = !string.IsNullOrWhiteSpace(roles)
			? roles.Split(',').Select(r => r.Trim()).ToArray()
			: [];
	}

	public async Task OnAuthorizationAsync(AuthorizationFilterContext context) {
		string? userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

		if (string.IsNullOrEmpty(userId)) {
			context.Result = new UnauthorizedResult();
			return;
		}


		var companyId = GetCompanyIdFromRequest(context);
		if (companyId is null) {
			companyId = Guid.Parse(_httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value!);
		}

		bool         hasAccess;

		if (_roles.Any()) {
			var userRoles = await _roleManager.Roles.ToListAsync();
			hasAccess = await _companyUserRepository.AnyAsync(cu =>
																  cu.UserId.ToString() == userId    &&
																  cu.CompanyId         == companyId &&
																  _roles.Contains(userRoles.FirstOrDefault(r => r.Id == cu.RoleId)!.Name));
		} else {
			hasAccess = await _companyUserRepository.AnyAsync(cu =>
																  cu.UserId.ToString() == userId &&
																  cu.CompanyId         == companyId);
		}

		if (hasAccess is false) {
			context.Result = new ForbidResult();
		}
	}

	private Guid? GetCompanyIdFromRequest(AuthorizationFilterContext context)
	{
		if (context.RouteData.Values.TryGetValue("companyId", out var value)) {
			var companyIdStr = value?.ToString();
			if (Guid.TryParse(companyIdStr, out Guid companyId))
				return companyId;
		}

		var companyIdQuery = context.HttpContext.Request.Query["companyId"].ToString();
		if (!string.IsNullOrEmpty(companyIdQuery) && Guid.TryParse(companyIdQuery, out Guid companyIdFromQuery))
			return companyIdFromQuery;

		if (context.HttpContext.Request.Method is "POST" or "PUT" or "DELETE") {
			var request = context.HttpContext.Request;
			request.EnableBuffering();

			using var reader = new StreamReader(request.Body, leaveOpen: true);
			var       body   = reader.ReadToEndAsync().Result;
			request.Body.Position = 0;

			var companyIdProp = new JsonElement();
			try {
				var requestObj = JsonSerializer.Deserialize<dynamic>(body);
				if (requestObj?.TryGetProperty("companyId", out companyIdProp)) {
					if (Guid.TryParse(companyIdProp.GetString(), out Guid companyIdFromBody))
						return companyIdFromBody;
				}
			} catch {
				// ignored
			}
		}
		return null;
	}
}