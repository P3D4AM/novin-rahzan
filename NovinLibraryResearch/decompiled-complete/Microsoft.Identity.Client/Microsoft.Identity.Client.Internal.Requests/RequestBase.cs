using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.TelemetryCore.Internal;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Internal.Requests;

internal abstract class RequestBase
{
	internal AuthenticationRequestParameters AuthenticationRequestParameters { get; }

	internal ICacheSessionManager CacheManager => AuthenticationRequestParameters.CacheSessionManager;

	protected IServiceBundle ServiceBundle { get; }

	protected RequestBase(IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, IAcquireTokenParameters acquireTokenParameters)
	{
		ServiceBundle = serviceBundle ?? throw new ArgumentNullException("serviceBundle");
		AuthenticationRequestParameters = authenticationRequestParameters ?? throw new ArgumentNullException("authenticationRequestParameters");
		if (acquireTokenParameters == null)
		{
			throw new ArgumentNullException("acquireTokenParameters");
		}
		ValidateScopeInput(authenticationRequestParameters.Scope);
		acquireTokenParameters.LogParameters(AuthenticationRequestParameters.RequestContext.Logger);
	}

	private void LogRequestStarted(AuthenticationRequestParameters authenticationRequestParameters)
	{
		string messageWithPii = string.Format(CultureInfo.InvariantCulture, "=== Token Acquisition ({3}) started:\n\tAuthority: {0}\n\tScope: {1}\n\tClientId: {2}\n\t", authenticationRequestParameters.AuthorityInfo?.CanonicalAuthority, authenticationRequestParameters.Scope.AsSingleString(), authenticationRequestParameters.ClientId, GetType().Name);
		string text = string.Format(CultureInfo.InvariantCulture, "=== Token Acquisition ({0}) started:\n\t", GetType().Name);
		if (authenticationRequestParameters.AuthorityInfo != null && KnownMetadataProvider.IsKnownEnvironment(authenticationRequestParameters.AuthorityInfo?.Host))
		{
			text += string.Format(CultureInfo.CurrentCulture, "\n\tAuthority Host: {0}", authenticationRequestParameters.AuthorityInfo?.Host);
		}
		authenticationRequestParameters.RequestContext.Logger.InfoPii(messageWithPii, text);
	}

	protected virtual SortedSet<string> GetOverridenScopes(ISet<string> inputScopes)
	{
		return null;
	}

	private void ValidateScopeInput(HashSet<string> scopesToValidate)
	{
		if (scopesToValidate.Contains(AuthenticationRequestParameters.ClientId))
		{
			throw new ArgumentException("API does not accept client id as a user-provided scope");
		}
	}

	protected abstract Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken);

	public async Task<AuthenticationResult> RunAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		ApiEvent apiEvent = InitializeApiEvent(AuthenticationRequestParameters.Account?.HomeAccountId?.Identifier);
		AuthenticationRequestParameters.RequestContext.ApiEvent = apiEvent;
		try
		{
			using (AuthenticationRequestParameters.RequestContext.CreateTelemetryHelper(apiEvent))
			{
				try
				{
					AuthenticationRequestParameters.LogParameters();
					LogRequestStarted(AuthenticationRequestParameters);
					AuthenticationResult authenticationResult = await ExecuteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					LogReturnedToken(authenticationResult);
					apiEvent.TenantId = authenticationResult.TenantId;
					apiEvent.AccountId = authenticationResult.UniqueId;
					apiEvent.WasSuccessful = true;
					return authenticationResult;
				}
				catch (MsalException ex)
				{
					apiEvent.ApiErrorCode = ex.ErrorCode;
					AuthenticationRequestParameters.RequestContext.Logger.ErrorPii(ex);
					throw;
				}
				catch (Exception exWithPii)
				{
					AuthenticationRequestParameters.RequestContext.Logger.ErrorPii(exWithPii);
					throw;
				}
			}
		}
		finally
		{
			ServiceBundle.MatsTelemetryManager.Flush(AuthenticationRequestParameters.RequestContext.CorrelationId.AsMatsCorrelationId());
		}
	}

	protected virtual void EnrichTelemetryApiEvent(ApiEvent apiEvent)
	{
	}

	private ApiEvent InitializeApiEvent(string accountId)
	{
		ApiEvent apiEvent = new ApiEvent(AuthenticationRequestParameters.RequestContext.Logger, ServiceBundle.PlatformProxy.CryptographyManager, AuthenticationRequestParameters.RequestContext.CorrelationId.AsMatsCorrelationId())
		{
			ApiId = AuthenticationRequestParameters.ApiId,
			ApiTelemId = AuthenticationRequestParameters.ApiTelemId,
			AccountId = (accountId ?? ""),
			WasSuccessful = false
		};
		foreach (KeyValuePair<string, string> apiTelemetryFeature in AuthenticationRequestParameters.GetApiTelemetryFeatures())
		{
			apiEvent[apiTelemetryFeature.Key] = apiTelemetryFeature.Value;
		}
		if (AuthenticationRequestParameters.AuthorityInfo != null)
		{
			apiEvent.Authority = new Uri(AuthenticationRequestParameters.AuthorityInfo.CanonicalAuthority);
			apiEvent.AuthorityType = AuthenticationRequestParameters.AuthorityInfo.AuthorityType.ToString();
		}
		EnrichTelemetryApiEvent(apiEvent);
		return apiEvent;
	}

	protected async Task<AuthenticationResult> CacheTokenResponseAndCreateAuthenticationResultAsync(MsalTokenResponse msalTokenResponse)
	{
		AuthenticationRequestParameters.RequestContext.Logger.Info("Checking client info returned from the server..");
		ClientInfo fromServer = null;
		if (!AuthenticationRequestParameters.IsClientCredentialRequest && AuthenticationRequestParameters.ApiId != ApiEvent.ApiIds.AcquireTokenByRefreshToken && AuthenticationRequestParameters.AuthorityInfo.AuthorityType != AuthorityType.Adfs)
		{
			fromServer = ClientInfo.CreateFromJson(msalTokenResponse.ClientInfo);
		}
		ValidateAccountIdentifiers(fromServer);
		IdToken idToken = IdToken.Parse(msalTokenResponse.IdToken);
		AuthenticationRequestParameters.TenantUpdatedCanonicalAuthority = Authority.CreateAuthorityWithTenant(AuthenticationRequestParameters.Authority.AuthorityInfo, idToken?.TenantId);
		AuthenticationRequestParameters.RequestContext.Logger.Info("Saving Token Response to cache..");
		Tuple<MsalAccessTokenCacheItem, MsalIdTokenCacheItem> obj = await CacheManager.SaveTokenResponseAsync(msalTokenResponse).ConfigureAwait(continueOnCapturedContext: false);
		MsalAccessTokenCacheItem item = obj.Item1;
		MsalIdTokenCacheItem item2 = obj.Item2;
		return new AuthenticationResult(item, item2, AuthenticationRequestParameters.AuthenticationScheme, AuthenticationRequestParameters.RequestContext.CorrelationId, msalTokenResponse.TokenSource);
	}

	private void ValidateAccountIdentifiers(ClientInfo fromServer)
	{
		if (fromServer == null || AuthenticationRequestParameters?.Account?.HomeAccountId == null || (AuthenticationRequestParameters.AuthorityInfo.AuthorityType == AuthorityType.B2C && fromServer.UniqueTenantIdentifier.Equals(AuthenticationRequestParameters.Account.HomeAccountId.TenantId, StringComparison.OrdinalIgnoreCase)) || (fromServer.UniqueObjectIdentifier.Equals(AuthenticationRequestParameters.Account.HomeAccountId.ObjectId, StringComparison.OrdinalIgnoreCase) && fromServer.UniqueTenantIdentifier.Equals(AuthenticationRequestParameters.Account.HomeAccountId.TenantId, StringComparison.OrdinalIgnoreCase)))
		{
			return;
		}
		AuthenticationRequestParameters.RequestContext.Logger.Error("Returned user identifiers do not match the sent user identifier");
		AuthenticationRequestParameters.RequestContext.Logger.ErrorPii(string.Format(CultureInfo.InvariantCulture, "Returned user identifiers (uid:{0} utid:{1}) does not match the sent user identifier (uid:{2} utid:{3})", fromServer.UniqueObjectIdentifier, fromServer.UniqueTenantIdentifier, AuthenticationRequestParameters.Account.HomeAccountId.ObjectId, AuthenticationRequestParameters.Account.HomeAccountId.TenantId), string.Empty);
		throw new MsalClientException("user_mismatch", "Returned user identifier does not match the sent user identifier when saving the token to the cache.");
	}

	protected async Task ResolveAuthorityEndpointsAsync()
	{
		await AuthorityEndpoints.UpdateAuthorityEndpointsAsync(AuthenticationRequestParameters).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal Task<MsalTokenResponse> SendTokenRequestAsync(IDictionary<string, string> additionalBodyParameters, CancellationToken cancellationToken)
	{
		return SendTokenRequestAsync(AuthenticationRequestParameters.Endpoints.TokenEndpoint, additionalBodyParameters, cancellationToken);
	}

	protected async Task<MsalTokenResponse> SendTokenRequestAsync(string tokenEndpoint, IDictionary<string, string> additionalBodyParameters, CancellationToken cancellationToken)
	{
		string scopeOverride = GetOverridenScopes(AuthenticationRequestParameters.Scope).AsSingleString();
		return await new TokenClient(AuthenticationRequestParameters).SendTokenRequestAsync(additionalBodyParameters, scopeOverride, tokenEndpoint, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void LogReturnedToken(AuthenticationResult result)
	{
		if (result.AccessToken != null)
		{
			AuthenticationRequestParameters.RequestContext.Logger.Info(string.Format(CultureInfo.InvariantCulture, "=== Token Acquisition finished successfully. An access token was returned with Expiration Time: {0} ===", result.ExpiresOn));
		}
	}
}
