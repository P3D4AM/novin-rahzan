using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization;

public static class AuthorizationServiceExtensions
{
	public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, object resource, IAuthorizationRequirement requirement)
	{
		if (service == null)
		{
			throw new ArgumentNullException("service");
		}
		if (requirement == null)
		{
			throw new ArgumentNullException("requirement");
		}
		return service.AuthorizeAsync(user, resource, new IAuthorizationRequirement[1] { requirement });
	}

	public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, object resource, AuthorizationPolicy policy)
	{
		if (service == null)
		{
			throw new ArgumentNullException("service");
		}
		if (policy == null)
		{
			throw new ArgumentNullException("policy");
		}
		return service.AuthorizeAsync(user, resource, policy.Requirements);
	}

	public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, AuthorizationPolicy policy)
	{
		if (service == null)
		{
			throw new ArgumentNullException("service");
		}
		if (policy == null)
		{
			throw new ArgumentNullException("policy");
		}
		return service.AuthorizeAsync(user, null, policy);
	}

	public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, string policyName)
	{
		if (service == null)
		{
			throw new ArgumentNullException("service");
		}
		if (policyName == null)
		{
			throw new ArgumentNullException("policyName");
		}
		return service.AuthorizeAsync(user, null, policyName);
	}
}
