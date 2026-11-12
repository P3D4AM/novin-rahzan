using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Authorization.Policy;

public class PolicyEvaluator : IPolicyEvaluator
{
	private readonly IAuthorizationService _authorization;

	public PolicyEvaluator(IAuthorizationService authorization)
	{
		_authorization = authorization;
	}

	public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
	{
		if (policy.AuthenticationSchemes != null && policy.AuthenticationSchemes.Count > 0)
		{
			ClaimsPrincipal newPrincipal = null;
			foreach (string authenticationScheme in policy.AuthenticationSchemes)
			{
				AuthenticateResult authenticateResult = await context.AuthenticateAsync(authenticationScheme);
				if (authenticateResult != null && authenticateResult.Succeeded)
				{
					newPrincipal = SecurityHelper.MergeUserPrincipal(newPrincipal, authenticateResult.Principal);
				}
			}
			if (newPrincipal != null)
			{
				context.User = newPrincipal;
				return AuthenticateResult.Success(new AuthenticationTicket(newPrincipal, string.Join(";", policy.AuthenticationSchemes)));
			}
			context.User = new ClaimsPrincipal(new ClaimsIdentity());
			return AuthenticateResult.NoResult();
		}
		return (context.User?.Identity?.IsAuthenticated ?? false) ? AuthenticateResult.Success(new AuthenticationTicket(context.User, "context.User")) : AuthenticateResult.NoResult();
	}

	public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object resource)
	{
		if (policy == null)
		{
			throw new ArgumentNullException("policy");
		}
		if ((await _authorization.AuthorizeAsync(context.User, resource, policy)).Succeeded)
		{
			return PolicyAuthorizationResult.Success();
		}
		return authenticationResult.Succeeded ? PolicyAuthorizationResult.Forbid() : PolicyAuthorizationResult.Challenge();
	}
}
