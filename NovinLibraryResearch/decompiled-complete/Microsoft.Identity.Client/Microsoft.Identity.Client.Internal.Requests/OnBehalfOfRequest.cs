using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class OnBehalfOfRequest : RequestBase
{
	private readonly AcquireTokenOnBehalfOfParameters _onBehalfOfParameters;

	public OnBehalfOfRequest(IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenOnBehalfOfParameters onBehalfOfParameters)
		: base(serviceBundle, authenticationRequestParameters, onBehalfOfParameters)
	{
		_onBehalfOfParameters = onBehalfOfParameters;
	}

	protected override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		if (base.AuthenticationRequestParameters.Scope == null || base.AuthenticationRequestParameters.Scope.Count == 0)
		{
			throw new MsalClientException("scopes_required_client_credentials", "At least one scope needs to be requested for this authentication flow.");
		}
		await ResolveAuthorityEndpointsAsync().ConfigureAwait(continueOnCapturedContext: false);
		MsalAccessTokenCacheItem msalAccessTokenItem = await base.CacheManager.FindAccessTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (msalAccessTokenItem != null)
		{
			MsalIdTokenCacheItem msalIdTokenCacheItem = await base.CacheManager.GetIdTokenCacheItemAsync(msalAccessTokenItem.GetIdTokenItemKey()).ConfigureAwait(continueOnCapturedContext: false);
			base.AuthenticationRequestParameters.RequestContext.Logger.Info("OBO found a valid access token in the cache. ID token also found? " + (msalIdTokenCacheItem != null));
			base.AuthenticationRequestParameters.RequestContext.ApiEvent.IsAccessTokenCacheHit = true;
			return new AuthenticationResult(msalAccessTokenItem, msalIdTokenCacheItem, base.AuthenticationRequestParameters.AuthenticationScheme, base.AuthenticationRequestParameters.RequestContext.CorrelationId, TokenSource.Cache);
		}
		return await CacheTokenResponseAndCreateAuthenticationResultAsync(await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected override void EnrichTelemetryApiEvent(ApiEvent apiEvent)
	{
		apiEvent.IsConfidentialClient = true;
	}

	private Dictionary<string, string> GetBodyParameters()
	{
		return new Dictionary<string, string>
		{
			["grant_type"] = _onBehalfOfParameters.UserAssertion.AssertionType,
			["assertion"] = _onBehalfOfParameters.UserAssertion.Assertion,
			["requested_token_use"] = "on_behalf_of"
		};
	}
}
