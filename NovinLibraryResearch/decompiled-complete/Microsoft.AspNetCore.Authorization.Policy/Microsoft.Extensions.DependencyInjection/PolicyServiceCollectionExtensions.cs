using System;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class PolicyServiceCollectionExtensions
{
	public static IServiceCollection AddAuthorizationPolicyEvaluator(this IServiceCollection services)
	{
		if (services == null)
		{
			throw new ArgumentNullException("services");
		}
		services.TryAdd(ServiceDescriptor.Transient<IPolicyEvaluator, PolicyEvaluator>());
		return services;
	}
}
