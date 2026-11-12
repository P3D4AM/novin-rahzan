using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Cache.Keys;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.TelemetryCore.Internal;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.Cache;

internal class CacheSessionManager : ICacheSessionManager
{
	private readonly AuthenticationRequestParameters _requestParams;

	private bool _cacheRefreshedForRead;

	public ITokenCacheInternal TokenCacheInternal { get; }

	public CacheSessionManager(ITokenCacheInternal tokenCacheInternal, AuthenticationRequestParameters requestParams)
	{
		TokenCacheInternal = tokenCacheInternal ?? throw new ArgumentNullException("tokenCacheInternal");
		_requestParams = requestParams ?? throw new ArgumentNullException("requestParams");
	}

	public async Task<MsalAccessTokenCacheItem> FindAccessTokenAsync()
	{
		await RefreshCacheForReadOperationsAsync(CacheEvent.TokenTypes.AT).ConfigureAwait(continueOnCapturedContext: false);
		return await TokenCacheInternal.FindAccessTokenAsync(_requestParams).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<Tuple<MsalAccessTokenCacheItem, MsalIdTokenCacheItem>> SaveTokenResponseAsync(MsalTokenResponse tokenResponse)
	{
		return await TokenCacheInternal.SaveTokenResponseAsync(_requestParams, tokenResponse).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<MsalIdTokenCacheItem> GetIdTokenCacheItemAsync(MsalIdTokenCacheKey idTokenCacheKey)
	{
		await RefreshCacheForReadOperationsAsync(CacheEvent.TokenTypes.ID).ConfigureAwait(continueOnCapturedContext: false);
		return TokenCacheInternal.GetIdTokenCacheItem(idTokenCacheKey);
	}

	public async Task<MsalRefreshTokenCacheItem> FindFamilyRefreshTokenAsync(string familyId)
	{
		await RefreshCacheForReadOperationsAsync(CacheEvent.TokenTypes.RT).ConfigureAwait(continueOnCapturedContext: false);
		if (string.IsNullOrEmpty(familyId))
		{
			throw new ArgumentNullException("familyId");
		}
		return await TokenCacheInternal.FindRefreshTokenAsync(_requestParams, familyId).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<MsalRefreshTokenCacheItem> FindRefreshTokenAsync()
	{
		await RefreshCacheForReadOperationsAsync(CacheEvent.TokenTypes.RT).ConfigureAwait(continueOnCapturedContext: false);
		return await TokenCacheInternal.FindRefreshTokenAsync(_requestParams).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<bool?> IsAppFociMemberAsync(string familyId)
	{
		await RefreshCacheForReadOperationsAsync(CacheEvent.TokenTypes.AppMetadata).ConfigureAwait(continueOnCapturedContext: false);
		return await TokenCacheInternal.IsFociMemberAsync(_requestParams, familyId).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<IEnumerable<IAccount>> GetAccountsAsync()
	{
		await RefreshCacheForReadOperationsAsync(CacheEvent.TokenTypes.Account).ConfigureAwait(continueOnCapturedContext: false);
		return await TokenCacheInternal.GetAccountsAsync(_requestParams).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task RefreshCacheForReadOperationsAsync(CacheEvent.TokenTypes cacheEventType)
	{
		if (_cacheRefreshedForRead)
		{
			return;
		}
		string correlationId = _requestParams.RequestContext.CorrelationId.AsMatsCorrelationId();
		CacheEvent cacheEvent = new CacheEvent("msal.token_cache_lookup", correlationId)
		{
			TokenType = cacheEventType
		};
		await TokenCacheInternal.Semaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			if (_cacheRefreshedForRead)
			{
				return;
			}
			using (_requestParams.RequestContext.CreateTelemetryHelper(cacheEvent))
			{
				string key = SuggestedWebCacheKeyFactory.GetKeyFromRequest(_requestParams);
				try
				{
					TokenCacheNotificationArgs args = new TokenCacheNotificationArgs(TokenCacheInternal, _requestParams.ClientId, _requestParams.Account, hasStateChanged: false, TokenCacheInternal.IsApplicationCache, TokenCacheInternal.HasTokensNoLocks(), key);
					await TokenCacheInternal.OnBeforeAccessAsync(args).ConfigureAwait(continueOnCapturedContext: false);
				}
				finally
				{
					TokenCacheNotificationArgs args2 = new TokenCacheNotificationArgs(TokenCacheInternal, _requestParams.ClientId, _requestParams.Account, hasStateChanged: false, TokenCacheInternal.IsApplicationCache, TokenCacheInternal.HasTokensNoLocks(), key);
					await TokenCacheInternal.OnAfterAccessAsync(args2).ConfigureAwait(continueOnCapturedContext: false);
				}
				_cacheRefreshedForRead = true;
			}
		}
		finally
		{
			TokenCacheInternal.Semaphore.Release();
		}
	}
}
