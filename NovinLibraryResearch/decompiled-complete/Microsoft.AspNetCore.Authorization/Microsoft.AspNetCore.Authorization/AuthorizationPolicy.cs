using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization;

public class AuthorizationPolicy
{
	public IReadOnlyList<IAuthorizationRequirement> Requirements { get; }

	public IReadOnlyList<string> AuthenticationSchemes { get; }

	public AuthorizationPolicy(IEnumerable<IAuthorizationRequirement> requirements, IEnumerable<string> authenticationSchemes)
	{
		if (requirements == null)
		{
			throw new ArgumentNullException("requirements");
		}
		if (authenticationSchemes == null)
		{
			throw new ArgumentNullException("authenticationSchemes");
		}
		if (requirements.Count() == 0)
		{
			throw new InvalidOperationException(Resources.Exception_AuthorizationPolicyEmpty);
		}
		Requirements = new List<IAuthorizationRequirement>(requirements).AsReadOnly();
		AuthenticationSchemes = new List<string>(authenticationSchemes).AsReadOnly();
	}

	public static AuthorizationPolicy Combine(params AuthorizationPolicy[] policies)
	{
		if (policies == null)
		{
			throw new ArgumentNullException("policies");
		}
		return Combine((IEnumerable<AuthorizationPolicy>)policies);
	}

	public static AuthorizationPolicy Combine(IEnumerable<AuthorizationPolicy> policies)
	{
		if (policies == null)
		{
			throw new ArgumentNullException("policies");
		}
		AuthorizationPolicyBuilder authorizationPolicyBuilder = new AuthorizationPolicyBuilder();
		foreach (AuthorizationPolicy policy in policies)
		{
			authorizationPolicyBuilder.Combine(policy);
		}
		return authorizationPolicyBuilder.Build();
	}

	public static async Task<AuthorizationPolicy> CombineAsync(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizeData> authorizeData)
	{
		if (policyProvider == null)
		{
			throw new ArgumentNullException("policyProvider");
		}
		if (authorizeData == null)
		{
			throw new ArgumentNullException("authorizeData");
		}
		AuthorizationPolicyBuilder policyBuilder = new AuthorizationPolicyBuilder();
		bool any = false;
		foreach (IAuthorizeData authorizeDatum in authorizeData)
		{
			any = true;
			bool flag = true;
			if (!string.IsNullOrWhiteSpace(authorizeDatum.Policy))
			{
				AuthorizationPolicy authorizationPolicy = await policyProvider.GetPolicyAsync(authorizeDatum.Policy);
				if (authorizationPolicy == null)
				{
					throw new InvalidOperationException(Resources.FormatException_AuthorizationPolicyNotFound(authorizeDatum.Policy));
				}
				policyBuilder.Combine(authorizationPolicy);
				flag = false;
			}
			string[] array = authorizeDatum.Roles?.Split(new char[1] { ',' });
			if (array != null && array.Any())
			{
				IEnumerable<string> roles = from r in array
					where !string.IsNullOrWhiteSpace(r)
					select r.Trim();
				policyBuilder.RequireRole(roles);
				flag = false;
			}
			string[] array2 = authorizeDatum.AuthenticationSchemes?.Split(new char[1] { ',' });
			if (array2 != null && array2.Any())
			{
				string[] array3 = array2;
				foreach (string text in array3)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						policyBuilder.AuthenticationSchemes.Add(text.Trim());
					}
				}
			}
			if (flag)
			{
				AuthorizationPolicyBuilder authorizationPolicyBuilder = policyBuilder;
				authorizationPolicyBuilder.Combine(await policyProvider.GetDefaultPolicyAsync());
			}
		}
		return any ? policyBuilder.Build() : null;
	}
}
