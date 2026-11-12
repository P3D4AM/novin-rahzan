using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class ConfidentialAuthCodeRequest : RequestBase
{
	private readonly AcquireTokenByAuthorizationCodeParameters _authorizationCodeParameters;

	public ConfidentialAuthCodeRequest(IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenByAuthorizationCodeParameters authorizationCodeParameters)
		: base(serviceBundle, authenticationRequestParameters, authorizationCodeParameters)
	{
		_authorizationCodeParameters = authorizationCodeParameters;
		RedirectUriHelper.Validate(authenticationRequestParameters.RedirectUri);
	}

	protected override void EnrichTelemetryApiEvent(ApiEvent apiEvent)
	{
		apiEvent.IsConfidentialClient = true;
	}

	protected override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		await ResolveAuthorityEndpointsAsync().ConfigureAwait(continueOnCapturedContext: false);
		return await CacheTokenResponseAndCreateAuthenticationResultAsync(await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
	}

	private Dictionary<string, string> GetBodyParameters()
	{
		return new Dictionary<string, string>
		{
			["grant_type"] = "authorization_code",
			["code"] = _authorizationCodeParameters.AuthorizationCode,
			["redirect_uri"] = base.AuthenticationRequestParameters.RedirectUri.OriginalString
		};
	}
}
