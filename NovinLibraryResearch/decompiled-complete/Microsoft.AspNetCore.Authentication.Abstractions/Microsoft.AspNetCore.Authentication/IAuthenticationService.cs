using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication;

public interface IAuthenticationService
{
	Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme);

	Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties);

	Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties);

	Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties);

	Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties);
}
