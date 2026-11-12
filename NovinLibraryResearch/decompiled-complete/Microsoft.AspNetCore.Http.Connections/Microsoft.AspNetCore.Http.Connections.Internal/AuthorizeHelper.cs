using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Http.Connections.Internal;

internal static class AuthorizeHelper
{
	public static async Task<bool> AuthorizeAsync(HttpContext context, IList<IAuthorizeData> policies)
	{
		if (policies.Count == 0)
		{
			return true;
		}
		AuthorizationPolicy authorizePolicy = await AuthorizationPolicy.CombineAsync(context.RequestServices.GetRequiredService<IAuthorizationPolicyProvider>(), policies);
		IPolicyEvaluator policyEvaluator = context.RequestServices.GetRequiredService<IPolicyEvaluator>();
		PolicyAuthorizationResult policyAuthorizationResult = await policyEvaluator.AuthorizeAsync(authorizePolicy, await policyEvaluator.AuthenticateAsync(authorizePolicy, context), context, null);
		if (policyAuthorizationResult.Succeeded)
		{
			return true;
		}
		if (policyAuthorizationResult.Challenged)
		{
			if (authorizePolicy.AuthenticationSchemes.Count <= 0)
			{
				await context.ChallengeAsync();
			}
			else
			{
				foreach (string authenticationScheme in authorizePolicy.AuthenticationSchemes)
				{
					await context.ChallengeAsync(authenticationScheme);
				}
			}
			return false;
		}
		if (policyAuthorizationResult.Forbidden)
		{
			if (authorizePolicy.AuthenticationSchemes.Count <= 0)
			{
				await context.ForbidAsync();
			}
			else
			{
				foreach (string authenticationScheme2 in authorizePolicy.AuthenticationSchemes)
				{
					await context.ForbidAsync(authenticationScheme2);
				}
			}
			return false;
		}
		return false;
	}
}
