using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication;

public interface IAuthenticationSignInHandler : IAuthenticationSignOutHandler, IAuthenticationHandler
{
	Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties);
}
