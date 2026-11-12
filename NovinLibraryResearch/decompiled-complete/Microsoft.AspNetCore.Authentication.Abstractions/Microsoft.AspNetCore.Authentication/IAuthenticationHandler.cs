using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication;

public interface IAuthenticationHandler
{
	Task InitializeAsync(AuthenticationScheme scheme, HttpContext context);

	Task<AuthenticateResult> AuthenticateAsync();

	Task ChallengeAsync(AuthenticationProperties properties);

	Task ForbidAsync(AuthenticationProperties properties);
}
