using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication;

public interface IAuthenticationSignOutHandler : IAuthenticationHandler
{
	Task SignOutAsync(AuthenticationProperties properties);
}
