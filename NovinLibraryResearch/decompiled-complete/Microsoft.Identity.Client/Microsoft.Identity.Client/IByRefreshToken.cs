using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client;

public interface IByRefreshToken
{
	AcquireTokenByRefreshTokenParameterBuilder AcquireTokenByRefreshToken(IEnumerable<string> scopes, string refreshToken);

	[Obsolete("Use AcquireTokenByRefreshToken instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenByRefreshTokenAsync(IEnumerable<string> scopes, string refreshToken);
}
