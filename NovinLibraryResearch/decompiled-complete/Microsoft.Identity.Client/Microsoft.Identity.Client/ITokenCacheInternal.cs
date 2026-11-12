using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Cache.Keys;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client;

internal interface ITokenCacheInternal : ITokenCache, ITokenCacheSerializer
{
	SemaphoreSlim Semaphore { get; }

	ILegacyCachePersistence LegacyPersistence { get; }

	ITokenCacheAccessor Accessor { get; }

	bool IsApplicationCache { get; }

	Task RemoveAccountAsync(IAccount account, RequestContext requestContext);

	Task<IEnumerable<IAccount>> GetAccountsAsync(AuthenticationRequestParameters requestParameters);

	Task<Tuple<MsalAccessTokenCacheItem, MsalIdTokenCacheItem>> SaveTokenResponseAsync(AuthenticationRequestParameters requestParams, MsalTokenResponse response);

	Task<MsalAccessTokenCacheItem> FindAccessTokenAsync(AuthenticationRequestParameters requestParams);

	MsalIdTokenCacheItem GetIdTokenCacheItem(MsalIdTokenCacheKey msalIdTokenCacheKey);

	Task<MsalRefreshTokenCacheItem> FindRefreshTokenAsync(AuthenticationRequestParameters requestParams, string familyId = null);

	Task<IEnumerable<MsalAccessTokenCacheItem>> GetAllAccessTokensAsync(bool filterByClientId);

	Task<IEnumerable<MsalRefreshTokenCacheItem>> GetAllRefreshTokensAsync(bool filterByClientId);

	Task<IEnumerable<MsalIdTokenCacheItem>> GetAllIdTokensAsync(bool filterByClientId);

	Task<IEnumerable<MsalAccountCacheItem>> GetAllAccountsAsync();

	void RemoveMsalAccountWithNoLocks(IAccount account, RequestContext requestContext);

	Task<bool?> IsFociMemberAsync(AuthenticationRequestParameters requestParams, string familyId);

	void SetIosKeychainSecurityGroup(string securityGroup);

	Task OnAfterAccessAsync(TokenCacheNotificationArgs args);

	Task OnBeforeAccessAsync(TokenCacheNotificationArgs args);

	Task OnBeforeWriteAsync(TokenCacheNotificationArgs args);

	bool HasTokensNoLocks();
}
