using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Authorization;

public class DefaultAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
	private readonly AuthorizationOptions _options;

	public DefaultAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
	{
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		_options = options.Value;
	}

	public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
	{
		return Task.FromResult(_options.DefaultPolicy);
	}

	public virtual Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
	{
		return Task.FromResult(_options.GetPolicy(policyName));
	}
}
