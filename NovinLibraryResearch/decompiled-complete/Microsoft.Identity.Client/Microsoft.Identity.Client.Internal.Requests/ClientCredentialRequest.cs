using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Internal.Requests;

internal class ClientCredentialRequest : RequestBase
{
	private readonly AcquireTokenForClientParameters _clientParameters;

	public ClientCredentialRequest(IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenForClientParameters clientParameters)
		: base(serviceBundle, authenticationRequestParameters, clientParameters)
	{
		_clientParameters = clientParameters;
	}

	protected override async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		if (base.AuthenticationRequestParameters.Scope == null || base.AuthenticationRequestParameters.Scope.Count == 0)
		{
			throw new MsalClientException("scopes_required_client_credentials", "At least one scope needs to be requested for this authentication flow.");
		}
		MsalAccessTokenCacheItem cachedAccessTokenItem = null;
		ICoreLogger logger = base.AuthenticationRequestParameters.RequestContext.Logger;
		if (!_clientParameters.ForceRefresh && string.IsNullOrEmpty(base.AuthenticationRequestParameters.Claims))
		{
			cachedAccessTokenItem = await base.CacheManager.FindAccessTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (cachedAccessTokenItem != null && !cachedAccessTokenItem.NeedsRefresh())
			{
				base.AuthenticationRequestParameters.RequestContext.ApiEvent.IsAccessTokenCacheHit = true;
				return new AuthenticationResult(cachedAccessTokenItem, null, base.AuthenticationRequestParameters.AuthenticationScheme, base.AuthenticationRequestParameters.RequestContext.CorrelationId, TokenSource.Cache);
			}
		}
		else
		{
			logger.Info("Skipped looking for an Access Token in the cache because ForceRefresh or Claims were set");
		}
		try
		{
			return await FetchNewAccessTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (MsalServiceException ex)
		{
			bool flag = ex.IsAadUnavailable();
			logger.Warning($"Fetching a new AT failed. Is AAD down? {flag}. Is there an AT in the cache that is usable? {cachedAccessTokenItem != null}");
			if (cachedAccessTokenItem != null && flag)
			{
				logger.Info("Returning existing access token. It is not expired, but should be refreshed.");
				return new AuthenticationResult(cachedAccessTokenItem, null, base.AuthenticationRequestParameters.AuthenticationScheme, base.AuthenticationRequestParameters.RequestContext.CorrelationId, TokenSource.Cache);
			}
			logger.Warning("Either the exception does not indicate a problem with AAD or the token cache does not have an AT that is usable.");
			throw;
		}
	}

	private async Task<AuthenticationResult> FetchNewAccessTokenAsync(CancellationToken cancellationToken)
	{
		await ResolveAuthorityEndpointsAsync().ConfigureAwait(continueOnCapturedContext: false);
		return await CacheTokenResponseAndCreateAuthenticationResultAsync(await SendTokenRequestAsync(GetBodyParameters(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected override void EnrichTelemetryApiEvent(ApiEvent apiEvent)
	{
		apiEvent.IsConfidentialClient = true;
	}

	protected override SortedSet<string> GetOverridenScopes(ISet<string> inputScopes)
	{
		return new SortedSet<string>(inputScopes);
	}

	private Dictionary<string, string> GetBodyParameters()
	{
		return new Dictionary<string, string>
		{
			["grant_type"] = "client_credentials",
			["scope"] = base.AuthenticationRequestParameters.Scope.AsSingleString()
		};
	}
}
