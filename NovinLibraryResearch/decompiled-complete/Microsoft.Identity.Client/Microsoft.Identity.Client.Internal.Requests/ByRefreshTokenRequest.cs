using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class ByRefreshTokenRequest : RequestBase
{
	private readonly AcquireTokenByRefreshTokenParameters _refreshTokenParameters;

	public ByRefreshTokenRequest(IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenByRefreshTokenParameters refreshTokenParameters)
		: base(serviceBundle, authenticationRequestParameters, refreshTokenParameters)
	{
		_refreshTokenParameters = refreshTokenParameters;
	}

	protected override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		base.AuthenticationRequestParameters.RequestContext.Logger.Verbose("Begin acquire token by refresh token...");
		await ResolveAuthorityEndpointsAsync().ConfigureAwait(continueOnCapturedContext: false);
		MsalTokenResponse msalTokenResponse = await SendTokenRequestAsync(GetBodyParameters(_refreshTokenParameters.RefreshToken), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (msalTokenResponse.RefreshToken == null)
		{
			base.AuthenticationRequestParameters.RequestContext.Logger.Info("Acquire by refresh token request completed, but no refresh token was found");
			throw new MsalServiceException(msalTokenResponse.Error, msalTokenResponse.ErrorDescription, null);
		}
		return await CacheTokenResponseAndCreateAuthenticationResultAsync(msalTokenResponse).ConfigureAwait(continueOnCapturedContext: false);
	}

	private Dictionary<string, string> GetBodyParameters(string refreshTokenSecret)
	{
		return new Dictionary<string, string>
		{
			["grant_type"] = "refresh_token",
			["refresh_token"] = refreshTokenSecret
		};
	}
}
