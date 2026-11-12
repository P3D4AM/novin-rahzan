using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Authorization;

public class AuthorizationOptions
{
	private IDictionary<string, AuthorizationPolicy> PolicyMap { get; } = new Dictionary<string, AuthorizationPolicy>(StringComparer.OrdinalIgnoreCase);


	public bool InvokeHandlersAfterFailure { get; set; } = true;


	public AuthorizationPolicy DefaultPolicy { get; set; } = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();


	public void AddPolicy(string name, AuthorizationPolicy policy)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (policy == null)
		{
			throw new ArgumentNullException("policy");
		}
		PolicyMap[name] = policy;
	}

	public void AddPolicy(string name, Action<AuthorizationPolicyBuilder> configurePolicy)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (configurePolicy == null)
		{
			throw new ArgumentNullException("configurePolicy");
		}
		AuthorizationPolicyBuilder authorizationPolicyBuilder = new AuthorizationPolicyBuilder();
		configurePolicy(authorizationPolicyBuilder);
		PolicyMap[name] = authorizationPolicyBuilder.Build();
	}

	public AuthorizationPolicy GetPolicy(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (!PolicyMap.ContainsKey(name))
		{
			return null;
		}
		return PolicyMap[name];
	}
}
