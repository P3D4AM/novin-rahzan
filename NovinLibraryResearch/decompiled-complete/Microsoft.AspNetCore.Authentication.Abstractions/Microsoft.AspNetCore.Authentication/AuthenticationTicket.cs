using System;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authentication;

public class AuthenticationTicket
{
	public string AuthenticationScheme { get; private set; }

	public ClaimsPrincipal Principal { get; private set; }

	public AuthenticationProperties Properties { get; private set; }

	public AuthenticationTicket(ClaimsPrincipal principal, AuthenticationProperties properties, string authenticationScheme)
	{
		if (principal == null)
		{
			throw new ArgumentNullException("principal");
		}
		AuthenticationScheme = authenticationScheme;
		Principal = principal;
		Properties = properties ?? new AuthenticationProperties();
	}

	public AuthenticationTicket(ClaimsPrincipal principal, string authenticationScheme)
		: this(principal, null, authenticationScheme)
	{
	}
}
