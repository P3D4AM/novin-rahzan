using System.Linq;
using System.Security.Claims;

namespace Microsoft.Extensions.Internal;

internal static class SecurityHelper
{
	public static ClaimsPrincipal MergeUserPrincipal(ClaimsPrincipal existingPrincipal, ClaimsPrincipal additionalPrincipal)
	{
		ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
		if (additionalPrincipal != null)
		{
			claimsPrincipal.AddIdentities(additionalPrincipal.Identities);
		}
		if (existingPrincipal != null)
		{
			claimsPrincipal.AddIdentities(existingPrincipal.Identities.Where((ClaimsIdentity i) => i.IsAuthenticated || i.Claims.Any()));
		}
		return claimsPrincipal;
	}
}
