using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Microsoft.AspNetCore.Authorization;

public class AuthorizationPolicyBuilder
{
	public IList<IAuthorizationRequirement> Requirements { get; set; } = new List<IAuthorizationRequirement>();


	public IList<string> AuthenticationSchemes { get; set; } = new List<string>();


	public AuthorizationPolicyBuilder(params string[] authenticationSchemes)
	{
		AddAuthenticationSchemes(authenticationSchemes);
	}

	public AuthorizationPolicyBuilder(AuthorizationPolicy policy)
	{
		Combine(policy);
	}

	public AuthorizationPolicyBuilder AddAuthenticationSchemes(params string[] schemes)
	{
		foreach (string item in schemes)
		{
			AuthenticationSchemes.Add(item);
		}
		return this;
	}

	public AuthorizationPolicyBuilder AddRequirements(params IAuthorizationRequirement[] requirements)
	{
		foreach (IAuthorizationRequirement item in requirements)
		{
			Requirements.Add(item);
		}
		return this;
	}

	public AuthorizationPolicyBuilder Combine(AuthorizationPolicy policy)
	{
		if (policy == null)
		{
			throw new ArgumentNullException("policy");
		}
		AddAuthenticationSchemes(policy.AuthenticationSchemes.ToArray());
		AddRequirements(policy.Requirements.ToArray());
		return this;
	}

	public AuthorizationPolicyBuilder RequireClaim(string claimType, params string[] requiredValues)
	{
		if (claimType == null)
		{
			throw new ArgumentNullException("claimType");
		}
		return RequireClaim(claimType, (IEnumerable<string>)requiredValues);
	}

	public AuthorizationPolicyBuilder RequireClaim(string claimType, IEnumerable<string> requiredValues)
	{
		if (claimType == null)
		{
			throw new ArgumentNullException("claimType");
		}
		Requirements.Add(new ClaimsAuthorizationRequirement(claimType, requiredValues));
		return this;
	}

	public AuthorizationPolicyBuilder RequireClaim(string claimType)
	{
		if (claimType == null)
		{
			throw new ArgumentNullException("claimType");
		}
		Requirements.Add(new ClaimsAuthorizationRequirement(claimType, null));
		return this;
	}

	public AuthorizationPolicyBuilder RequireRole(params string[] roles)
	{
		if (roles == null)
		{
			throw new ArgumentNullException("roles");
		}
		return RequireRole((IEnumerable<string>)roles);
	}

	public AuthorizationPolicyBuilder RequireRole(IEnumerable<string> roles)
	{
		if (roles == null)
		{
			throw new ArgumentNullException("roles");
		}
		Requirements.Add(new RolesAuthorizationRequirement(roles));
		return this;
	}

	public AuthorizationPolicyBuilder RequireUserName(string userName)
	{
		if (userName == null)
		{
			throw new ArgumentNullException("userName");
		}
		Requirements.Add(new NameAuthorizationRequirement(userName));
		return this;
	}

	public AuthorizationPolicyBuilder RequireAuthenticatedUser()
	{
		Requirements.Add(new DenyAnonymousAuthorizationRequirement());
		return this;
	}

	public AuthorizationPolicyBuilder RequireAssertion(Func<AuthorizationHandlerContext, bool> handler)
	{
		if (handler == null)
		{
			throw new ArgumentNullException("handler");
		}
		Requirements.Add(new AssertionRequirement(handler));
		return this;
	}

	public AuthorizationPolicyBuilder RequireAssertion(Func<AuthorizationHandlerContext, Task<bool>> handler)
	{
		if (handler == null)
		{
			throw new ArgumentNullException("handler");
		}
		Requirements.Add(new AssertionRequirement(handler));
		return this;
	}

	public AuthorizationPolicy Build()
	{
		return new AuthorizationPolicy(Requirements, AuthenticationSchemes.Distinct());
	}
}
