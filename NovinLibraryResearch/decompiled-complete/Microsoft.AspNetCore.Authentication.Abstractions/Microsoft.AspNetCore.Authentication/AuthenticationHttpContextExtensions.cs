using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Authentication;

public static class AuthenticationHttpContextExtensions
{
	public static Task<AuthenticateResult> AuthenticateAsync(this HttpContext context)
	{
		return context.AuthenticateAsync(null);
	}

	public static Task<AuthenticateResult> AuthenticateAsync(this HttpContext context, string scheme)
	{
		return context.RequestServices.GetRequiredService<IAuthenticationService>().AuthenticateAsync(context, scheme);
	}

	public static Task ChallengeAsync(this HttpContext context, string scheme)
	{
		return context.ChallengeAsync(scheme, null);
	}

	public static Task ChallengeAsync(this HttpContext context)
	{
		return context.ChallengeAsync(null, null);
	}

	public static Task ChallengeAsync(this HttpContext context, AuthenticationProperties properties)
	{
		return context.ChallengeAsync(null, properties);
	}

	public static Task ChallengeAsync(this HttpContext context, string scheme, AuthenticationProperties properties)
	{
		return context.RequestServices.GetRequiredService<IAuthenticationService>().ChallengeAsync(context, scheme, properties);
	}

	public static Task ForbidAsync(this HttpContext context, string scheme)
	{
		return context.ForbidAsync(scheme, null);
	}

	public static Task ForbidAsync(this HttpContext context)
	{
		return context.ForbidAsync(null, null);
	}

	public static Task ForbidAsync(this HttpContext context, AuthenticationProperties properties)
	{
		return context.ForbidAsync(null, properties);
	}

	public static Task ForbidAsync(this HttpContext context, string scheme, AuthenticationProperties properties)
	{
		return context.RequestServices.GetRequiredService<IAuthenticationService>().ForbidAsync(context, scheme, properties);
	}

	public static Task SignInAsync(this HttpContext context, string scheme, ClaimsPrincipal principal)
	{
		return context.SignInAsync(scheme, principal, null);
	}

	public static Task SignInAsync(this HttpContext context, ClaimsPrincipal principal)
	{
		return context.SignInAsync(null, principal, null);
	}

	public static Task SignInAsync(this HttpContext context, ClaimsPrincipal principal, AuthenticationProperties properties)
	{
		return context.SignInAsync(null, principal, properties);
	}

	public static Task SignInAsync(this HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
	{
		return context.RequestServices.GetRequiredService<IAuthenticationService>().SignInAsync(context, scheme, principal, properties);
	}

	public static Task SignOutAsync(this HttpContext context)
	{
		return context.SignOutAsync(null, null);
	}

	public static Task SignOutAsync(this HttpContext context, AuthenticationProperties properties)
	{
		return context.SignOutAsync(null, properties);
	}

	public static Task SignOutAsync(this HttpContext context, string scheme)
	{
		return context.SignOutAsync(scheme, null);
	}

	public static Task SignOutAsync(this HttpContext context, string scheme, AuthenticationProperties properties)
	{
		return context.RequestServices.GetRequiredService<IAuthenticationService>().SignOutAsync(context, scheme, properties);
	}

	public static Task<string> GetTokenAsync(this HttpContext context, string scheme, string tokenName)
	{
		return context.RequestServices.GetRequiredService<IAuthenticationService>().GetTokenAsync(context, scheme, tokenName);
	}

	public static Task<string> GetTokenAsync(this HttpContext context, string tokenName)
	{
		return context.RequestServices.GetRequiredService<IAuthenticationService>().GetTokenAsync(context, tokenName);
	}
}
