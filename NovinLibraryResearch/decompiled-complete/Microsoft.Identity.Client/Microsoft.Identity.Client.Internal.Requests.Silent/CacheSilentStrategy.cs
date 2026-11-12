using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Internal.Requests.Silent;

internal class CacheSilentStrategy : ISilentAuthRequestStrategy
{
	private readonly AcquireTokenSilentParameters _silentParameters;

	private const string TheOnlyFamilyId = "1";

	private readonly SilentRequest _silentRequest;

	private AuthenticationRequestParameters AuthenticationRequestParameters { get; }

	private ICacheSessionManager CacheManager => AuthenticationRequestParameters.CacheSessionManager;

	protected IServiceBundle ServiceBundle { get; }

	public CacheSilentStrategy(SilentRequest request, IServiceBundle serviceBundle, AuthenticationRequestParameters authenticationRequestParameters, AcquireTokenSilentParameters silentParameters)
	{
		AuthenticationRequestParameters = authenticationRequestParameters;
		_silentParameters = silentParameters;
		ServiceBundle = serviceBundle;
		_silentRequest = request;
	}

	public async Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		ICoreLogger logger = AuthenticationRequestParameters.RequestContext.Logger;
		MsalAccessTokenCacheItem cachedAccessTokenItem = null;
		ThrowIfNoScopesOnB2C();
		if (!_silentParameters.ForceRefresh && string.IsNullOrEmpty(AuthenticationRequestParameters.Claims))
		{
			cachedAccessTokenItem = await CacheManager.FindAccessTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (cachedAccessTokenItem != null && !cachedAccessTokenItem.NeedsRefresh())
			{
				logger.Info("Returning access token found in cache. RefreshOn exists ? " + cachedAccessTokenItem.RefreshOn.HasValue);
				AuthenticationRequestParameters.RequestContext.ApiEvent.IsAccessTokenCacheHit = true;
				return await CreateAuthenticationResultAsync(cachedAccessTokenItem).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		else
		{
			logger.Info("Skipped looking for an Access Token because ForceRefresh or Claims were set");
		}
		AuthenticationResult result = default(AuthenticationResult);
		try
		{
			result = await RefreshRtOrFailAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return result;
		}
		catch (MsalServiceException ex)
		{
			MsalServiceException e = ex;
			if ("bad_token".Equals(e.SubError, StringComparison.OrdinalIgnoreCase))
			{
				await CacheManager.TokenCacheInternal.RemoveAccountAsync(AuthenticationRequestParameters.Account, AuthenticationRequestParameters.RequestContext).ConfigureAwait(continueOnCapturedContext: false);
				logger.Warning("Failed to refresh access token because the refresh token is invalid, removing account from cache.");
				throw;
			}
			bool flag = e.IsAadUnavailable();
			logger.Warning($"Refreshing the RT failed. Is AAD down? {flag}. Is there an AT in the cache that is usable? {cachedAccessTokenItem != null}");
			if (cachedAccessTokenItem != null && flag)
			{
				logger.Info("Returning existing access token. It is not expired, but should be refreshed.");
				return await CreateAuthenticationResultAsync(cachedAccessTokenItem).ConfigureAwait(continueOnCapturedContext: false);
			}
			logger.Warning("Failed to refresh the RT and cannot use existing AT (expired or missing).");
			ExceptionDispatchInfo.Capture((ex as Exception) ?? throw ex).Throw();
		}
		return result;
	}

	private async Task<AuthenticationResult> RefreshRtOrFailAsync(CancellationToken cancellationToken)
	{
		MsalTokenResponse msalTokenResponse = await TryGetTokenUsingFociAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (msalTokenResponse == null)
		{
			msalTokenResponse = await RefreshAccessTokenAsync(await FindRefreshTokenOrFailAsync().ConfigureAwait(continueOnCapturedContext: false), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await _silentRequest.CacheTokenResponseAndCreateAuthenticationResultAsync(msalTokenResponse).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<AuthenticationResult> CreateAuthenticationResultAsync(MsalAccessTokenCacheItem cachedAccessTokenItem)
	{
		return new AuthenticationResult(cachedAccessTokenItem, await CacheManager.GetIdTokenCacheItemAsync(cachedAccessTokenItem.GetIdTokenItemKey()).ConfigureAwait(continueOnCapturedContext: false), AuthenticationRequestParameters.AuthenticationScheme, AuthenticationRequestParameters.RequestContext.CorrelationId, TokenSource.Cache);
	}

	private void ThrowIfNoScopesOnB2C()
	{
		if (!AuthenticationRequestParameters.HasScopes && AuthenticationRequestParameters.AuthorityInfo.AuthorityType == AuthorityType.B2C)
		{
			throw new MsalUiRequiredException("scopes_required_client_credentials", "At least one scope needs to be requested for this authentication flow.", null, UiRequiredExceptionClassification.AcquireTokenSilentFailed);
		}
	}

	private async Task<MsalTokenResponse> TryGetTokenUsingFociAsync(CancellationToken cancellationToken)
	{
		if (!ServiceBundle.PlatformProxy.GetFeatureFlags().IsFociEnabled)
		{
			return null;
		}
		ICoreLogger logger = AuthenticationRequestParameters.RequestContext.Logger;
		bool? flag = await CacheManager.IsAppFociMemberAsync("1").ConfigureAwait(continueOnCapturedContext: false);
		if (flag.HasValue && !flag.Value)
		{
			AuthenticationRequestParameters.RequestContext.Logger.Verbose("[FOCI] App is not part of the family, skipping FOCI.");
			return null;
		}
		logger.Verbose("[FOCI] App is part of the family or unknown, looking for FRT");
		MsalRefreshTokenCacheItem msalRefreshTokenCacheItem = await CacheManager.FindFamilyRefreshTokenAsync("1").ConfigureAwait(continueOnCapturedContext: false);
		logger.Verbose("[FOCI] FRT found? " + (msalRefreshTokenCacheItem != null));
		if (msalRefreshTokenCacheItem != null)
		{
			try
			{
				MsalTokenResponse result = await RefreshAccessTokenAsync(msalRefreshTokenCacheItem, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				logger.Verbose("[FOCI] FRT refresh succeeded");
				return result;
			}
			catch (MsalServiceException ex)
			{
				if ("invalid_grant".Equals(ex?.ErrorCode, StringComparison.OrdinalIgnoreCase) && "client_mismatch".Equals(ex?.SubError, StringComparison.OrdinalIgnoreCase))
				{
					logger.Error("[FOCI] FRT refresh failed - client mismatch");
					return null;
				}
				logger.Error("[FOCI] FRT refresh failed - other error");
				throw;
			}
		}
		return null;
	}

	private async Task<MsalTokenResponse> RefreshAccessTokenAsync(MsalRefreshTokenCacheItem msalRefreshTokenItem, CancellationToken cancellationToken)
	{
		AuthenticationRequestParameters.RequestContext.Logger.Verbose("Refreshing access token...");
		await AuthorityEndpoints.UpdateAuthorityEndpointsAsync(AuthenticationRequestParameters).ConfigureAwait(continueOnCapturedContext: false);
		MsalTokenResponse msalTokenResponse = await _silentRequest.SendTokenRequestAsync(GetBodyParameters(msalRefreshTokenItem.Secret), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (msalTokenResponse.RefreshToken == null)
		{
			msalTokenResponse.RefreshToken = msalRefreshTokenItem.Secret;
			AuthenticationRequestParameters.RequestContext.Logger.Info("Refresh token was missing from the token refresh response, so the refresh token in the request is returned instead");
		}
		return msalTokenResponse;
	}

	private async Task<MsalRefreshTokenCacheItem> FindRefreshTokenOrFailAsync()
	{
		MsalRefreshTokenCacheItem obj = await CacheManager.FindRefreshTokenAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (obj == null)
		{
			AuthenticationRequestParameters.RequestContext.Logger.Verbose("No Refresh Token was found in the cache");
			throw new MsalUiRequiredException("no_tokens_found", "No Refresh Token found in the cache", null, UiRequiredExceptionClassification.AcquireTokenSilentFailed);
		}
		return obj;
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
