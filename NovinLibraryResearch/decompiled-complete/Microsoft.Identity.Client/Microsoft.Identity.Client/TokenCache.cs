using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Cache.Keys;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Requests;
using Microsoft.Identity.Client.OAuth2;
using Microsoft.Identity.Client.PlatformsCommon.Factories;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client;

public sealed class TokenCache : ITokenCacheInternal, ITokenCache, ITokenCacheSerializer
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use Microsoft.Identity.Client.TokenCacheCallback instead. See https://aka.msa/msal-net-3x-cache-breaking-change", true)]
	public delegate void TokenCacheNotification(TokenCacheNotificationArgs args);

	internal const string NullPreferredUsernameDisplayLabel = "Missing from the token response";

	private static readonly TimeSpan AccessTokenExpirationBuffer = TimeSpan.FromMinutes(5.0);

	internal const int ExpirationTooLongInDays = 3650;

	private readonly ITokenCacheBlobStorage _defaultTokenCacheBlobStorage;

	private readonly IFeatureFlags _featureFlags;

	private readonly ITokenCacheAccessor _accessor;

	private volatile bool _hasStateChanged;

	private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

	private IDictionary<string, JToken> _unknownNodes;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Please use the equivalent flag TokenCacheNotificationArgs.HasStateChanged, which indicates if the operation triggering the notification is modifying the cache or not. Setting the flag is not required.")]
	public bool HasStateChanged
	{
		get
		{
			return _hasStateChanged;
		}
		set
		{
			_hasStateChanged = value;
		}
	}

	private ICoreLogger Logger => ServiceBundle.DefaultLogger;

	internal IServiceBundle ServiceBundle { get; }

	internal ILegacyCachePersistence LegacyCachePersistence { get; }

	internal string ClientId => ServiceBundle.Config.ClientId;

	ITokenCacheAccessor ITokenCacheInternal.Accessor => _accessor;

	ILegacyCachePersistence ITokenCacheInternal.LegacyPersistence => LegacyCachePersistence;

	private bool IsAppTokenCache { get; }

	bool ITokenCacheInternal.IsApplicationCache => IsAppTokenCache;

	SemaphoreSlim ITokenCacheInternal.Semaphore => _semaphoreSlim;

	internal TokenCacheCallback BeforeAccess { get; set; }

	internal TokenCacheCallback BeforeWrite { get; set; }

	internal TokenCacheCallback AfterAccess { get; set; }

	internal Func<TokenCacheNotificationArgs, Task> AsyncBeforeAccess { get; set; }

	internal Func<TokenCacheNotificationArgs, Task> AsyncAfterAccess { get; set; }

	internal Func<TokenCacheNotificationArgs, Task> AsyncBeforeWrite { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is expected to be removed in MSAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
	public CacheData SerializeUnifiedAndAdalCache()
	{
		GuardOnMobilePlatforms();
		_semaphoreSlim.Wait();
		try
		{
			byte[] unifiedState = Serialize();
			byte[] adalV3State = LegacyCachePersistence.LoadCache();
			return new CacheData
			{
				AdalV3State = adalV3State,
				UnifiedState = unifiedState
			};
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is expected to be removed in MSAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
	public void DeserializeUnifiedAndAdalCache(CacheData cacheData)
	{
		GuardOnMobilePlatforms();
		_semaphoreSlim.Wait();
		try
		{
			Deserialize(cacheData.UnifiedState);
			LegacyCachePersistence.WriteCache(cacheData.AdalV3State);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is expected to be removed in MSAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
	public byte[] Serialize()
	{
		throw new NotImplementedException();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is expected to be removed in MSAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
	public void Deserialize(byte[] msalV2State)
	{
		throw new NotImplementedException();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	public byte[] SerializeAdalV3()
	{
		throw new NotImplementedException("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	public void DeserializeAdalV3(byte[] adalV3State)
	{
		throw new NotImplementedException("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	public byte[] SerializeMsalV2()
	{
		throw new NotImplementedException("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	public void DeserializeMsalV2(byte[] msalV2State)
	{
		throw new NotImplementedException("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	public byte[] SerializeMsalV3()
	{
		throw new NotImplementedException("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	public void DeserializeMsalV3(byte[] msalV3State, bool shouldClearExistingCache)
	{
		throw new NotImplementedException("This is removed in MSAL.NET v4. Read more: https://aka.ms/msal-net-4x-cache-breaking-change");
	}

	[Obsolete("The recommended way to get a cache is by using IClientApplicationBase.UserTokenCache or IClientApplicationBase.AppTokenCache")]
	public TokenCache()
		: this(null, isApplicationTokenCache: false)
	{
	}

	internal TokenCache(IServiceBundle serviceBundle, bool isApplicationTokenCache)
	{
		IPlatformProxy platformProxy = serviceBundle?.PlatformProxy ?? PlatformProxyFactory.CreatePlatformProxy(null);
		_accessor = platformProxy.CreateTokenCacheAccessor();
		_featureFlags = platformProxy.GetFeatureFlags();
		_defaultTokenCacheBlobStorage = platformProxy.CreateTokenCacheBlobStorage();
		if (_defaultTokenCacheBlobStorage != null)
		{
			BeforeAccess = _defaultTokenCacheBlobStorage.OnBeforeAccess;
			AfterAccess = _defaultTokenCacheBlobStorage.OnAfterAccess;
			BeforeWrite = _defaultTokenCacheBlobStorage.OnBeforeWrite;
			AsyncBeforeAccess = null;
			AsyncAfterAccess = null;
			AsyncBeforeWrite = null;
		}
		LegacyCachePersistence = platformProxy.CreateLegacyCachePersistence();
		IsAppTokenCache = isApplicationTokenCache;
		ServiceBundle = serviceBundle;
	}

	internal TokenCache(IServiceBundle serviceBundle, ILegacyCachePersistence legacyCachePersistenceForTest, bool isApplicationTokenCache)
		: this(serviceBundle, isApplicationTokenCache)
	{
		LegacyCachePersistence = legacyCachePersistenceForTest;
	}

	public void SetIosKeychainSecurityGroup(string securityGroup)
	{
	}

	private void UpdateAppMetadata(string clientId, string environment, string familyId)
	{
		if (_featureFlags.IsFociEnabled)
		{
			MsalAppMetadataCacheItem item = new MsalAppMetadataCacheItem(clientId, environment, familyId);
			_accessor.SaveAppMetadata(item);
		}
	}

	private void DeleteAccessTokensWithIntersectingScopes(AuthenticationRequestParameters requestParams, IEnumerable<string> environmentAliases, string tenantId, HashSet<string> scopeSet, string homeAccountId, string tokenType)
	{
		requestParams.RequestContext.Logger.Info("Looking for scopes for the authority in the cache which intersect with " + requestParams.Scope.AsSingleString());
		IList<MsalAccessTokenCacheItem> list = new List<MsalAccessTokenCacheItem>();
		foreach (MsalAccessTokenCacheItem allAccessToken in _accessor.GetAllAccessTokens())
		{
			if (allAccessToken.ClientId.Equals(ClientId, StringComparison.OrdinalIgnoreCase) && environmentAliases.Contains(allAccessToken.Environment) && string.Equals(allAccessToken.TokenType ?? "", tokenType ?? "", StringComparison.OrdinalIgnoreCase) && (allAccessToken.IsAdfs || allAccessToken.TenantId.Equals(tenantId, StringComparison.OrdinalIgnoreCase)) && allAccessToken.ScopeSet.Overlaps(scopeSet))
			{
				requestParams.RequestContext.Logger.Verbose("Intersecting scopes found");
				list.Add(allAccessToken);
			}
		}
		requestParams.RequestContext.Logger.Info("Intersecting scope entries count - " + list.Count);
		if (!requestParams.IsClientCredentialRequest)
		{
			list = list.Where((MsalAccessTokenCacheItem item) => item.HomeAccountId.Equals(homeAccountId, StringComparison.OrdinalIgnoreCase)).ToList();
			requestParams.RequestContext.Logger.Info("Matching entries after filtering by user - " + list.Count);
		}
		foreach (MsalAccessTokenCacheItem item in list)
		{
			_accessor.DeleteAccessToken(item.GetKey());
		}
	}

	private string GetAccessTokenExpireLogMessageContent(MsalAccessTokenCacheItem msalAccessTokenCacheItem)
	{
		return string.Format(CultureInfo.InvariantCulture, "[Current time ({0}) - Expiration Time ({1}) - Extended Expiration Time ({2})]", DateTime.UtcNow, msalAccessTokenCacheItem.ExpiresOn, msalAccessTokenCacheItem.ExtendedExpiresOn);
	}

	private bool RtMatchesAccount(MsalRefreshTokenCacheItem rtItem, MsalAccountCacheItem account)
	{
		bool num = rtItem.HomeAccountId.Equals(account.HomeAccountId, StringComparison.OrdinalIgnoreCase);
		bool flag = rtItem.IsFRT || rtItem.ClientId.Equals(ClientId, StringComparison.OrdinalIgnoreCase);
		return num && flag;
	}

	private static void UpdateMapWithAdalAccountsWithClientInfo(string envFromRequest, IEnumerable<string> envAliases, AdalUsersForMsal adalUsers, IDictionary<string, Account> clientInfoToAccountMap)
	{
		foreach (KeyValuePair<string, AdalUserInfo> item in adalUsers.GetUsersWithClientInfo(envAliases))
		{
			string text = ClientInfo.CreateFromJson(item.Key).ToAccountIdentifier();
			if (!clientInfoToAccountMap.ContainsKey(text))
			{
				clientInfoToAccountMap[text] = new Account(text, item.Value.DisplayableId, envFromRequest);
			}
		}
	}

	private static List<IAccount> UpdateWithAdalAccountsWithoutClientInfo(string envFromRequest, IEnumerable<string> envAliases, AdalUsersForMsal adalUsers, IDictionary<string, Account> clientInfoToAccountMap)
	{
		List<IAccount> list = new List<IAccount>();
		list.AddRange(clientInfoToAccountMap.Values);
		List<string> list2 = clientInfoToAccountMap.Values.Select((Account o) => o.Username).Distinct().ToList();
		foreach (AdalUserInfo item in adalUsers.GetUsersWithoutClientInfo(envAliases))
		{
			if (!string.IsNullOrEmpty(item.DisplayableId) && !list2.Contains(item.DisplayableId))
			{
				list.Add(new Account(null, item.DisplayableId, envFromRequest));
				list2.Add(item.DisplayableId);
			}
		}
		return list;
	}

	private IEnumerable<MsalRefreshTokenCacheItem> GetAllRefreshTokensWithNoLocks(bool filterByClientId)
	{
		IEnumerable<MsalRefreshTokenCacheItem> allRefreshTokens = _accessor.GetAllRefreshTokens();
		if (!filterByClientId)
		{
			return allRefreshTokens;
		}
		return allRefreshTokens.Where((MsalRefreshTokenCacheItem x) => x.ClientId.Equals(ClientId, StringComparison.OrdinalIgnoreCase));
	}

	private IEnumerable<MsalAccessTokenCacheItem> GetAllAccessTokensWithNoLocks(bool filterByClientId)
	{
		IEnumerable<MsalAccessTokenCacheItem> allAccessTokens = _accessor.GetAllAccessTokens();
		if (!filterByClientId)
		{
			return allAccessTokens;
		}
		return allAccessTokens.Where((MsalAccessTokenCacheItem x) => x.ClientId.Equals(ClientId, StringComparison.OrdinalIgnoreCase));
	}

	private IEnumerable<MsalIdTokenCacheItem> GetAllIdTokensWithNoLocks(bool filterByClientId)
	{
		IEnumerable<MsalIdTokenCacheItem> allIdTokens = _accessor.GetAllIdTokens();
		if (!filterByClientId)
		{
			return allIdTokens;
		}
		return allIdTokens.Where((MsalIdTokenCacheItem x) => x.ClientId.Equals(ClientId, StringComparison.OrdinalIgnoreCase));
	}

	private static bool FrtExists(List<MsalRefreshTokenCacheItem> allRefreshTokens)
	{
		return allRefreshTokens.Any((MsalRefreshTokenCacheItem rt) => rt.IsFRT);
	}

	private void RemoveAdalUser(IAccount account)
	{
		CacheFallbackOperations.RemoveAdalUser(Logger, LegacyCachePersistence, ClientId, account.Username, account.HomeAccountId?.Identifier);
	}

	async Task<Tuple<MsalAccessTokenCacheItem, MsalIdTokenCacheItem>> ITokenCacheInternal.SaveTokenResponseAsync(AuthenticationRequestParameters requestParams, MsalTokenResponse response)
	{
		MsalAccessTokenCacheItem msalAccessTokenCacheItem = null;
		MsalRefreshTokenCacheItem msalRefreshTokenCacheItem = null;
		MsalIdTokenCacheItem msalIdTokenCacheItem = null;
		MsalAccountCacheItem msalAccountCacheItem = null;
		IdToken idToken = IdToken.Parse(response.IdToken);
		if (idToken == null)
		{
			requestParams.RequestContext.Logger.Info("ID Token not present in response.");
		}
		string tenantId = Authority.CreateAuthority(requestParams.TenantUpdatedCanonicalAuthority.AuthorityInfo.CanonicalAuthority).TenantId;
		bool isAdfsAuthority = requestParams.AuthorityInfo.AuthorityType == AuthorityType.Adfs;
		string preferredUsername = GetPreferredUsernameFromIdToken(isAdfsAuthority, idToken);
		string username = ((!isAdfsAuthority) ? preferredUsername : idToken?.Upn);
		string homeAccountId = GetHomeAccountId(requestParams, response, idToken);
		string suggestedWebCacheKey = SuggestedWebCacheKeyFactory.GetKeyFromResponse(requestParams, homeAccountId);
		InstanceDiscoveryMetadataEntry instanceDiscoveryMetadata = await ServiceBundle.InstanceDiscoveryManager.GetMetadataEntryAsync(requestParams.TenantUpdatedCanonicalAuthority.AuthorityInfo.CanonicalAuthority, requestParams.RequestContext).ConfigureAwait(continueOnCapturedContext: false);
		if (!string.IsNullOrEmpty(response.AccessToken))
		{
			msalAccessTokenCacheItem = new MsalAccessTokenCacheItem(instanceDiscoveryMetadata.PreferredCache, requestParams.ClientId, response, tenantId, homeAccountId, requestParams.AuthenticationScheme.KeyId)
			{
				UserAssertionHash = requestParams.UserAssertion?.AssertionHash,
				IsAdfs = isAdfsAuthority
			};
		}
		if (!string.IsNullOrEmpty(response.RefreshToken))
		{
			msalRefreshTokenCacheItem = new MsalRefreshTokenCacheItem(instanceDiscoveryMetadata.PreferredCache, requestParams.ClientId, response, homeAccountId);
			if (!_featureFlags.IsFociEnabled)
			{
				msalRefreshTokenCacheItem.FamilyId = null;
			}
		}
		if (idToken != null)
		{
			msalIdTokenCacheItem = new MsalIdTokenCacheItem(instanceDiscoveryMetadata.PreferredCache, requestParams.ClientId, response, tenantId, homeAccountId)
			{
				IsAdfs = isAdfsAuthority
			};
			Dictionary<string, string> wamAccountIds = GetWamAccountIds(requestParams, response);
			msalAccountCacheItem = new MsalAccountCacheItem(instanceDiscoveryMetadata.PreferredCache, response.ClientInfo, homeAccountId, idToken, preferredUsername, tenantId, wamAccountIds);
		}
		Account account = new Account(homeAccountId, username, instanceDiscoveryMetadata.PreferredCache);
		await _semaphoreSlim.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			TokenCacheNotificationArgs args = new TokenCacheNotificationArgs(this, ClientId, account, hasStateChanged: true, ((ITokenCacheInternal)this).IsApplicationCache, ((ITokenCacheInternal)this).HasTokensNoLocks(), suggestedWebCacheKey);
			HasStateChanged = true;
			try
			{
				await ((ITokenCacheInternal)this).OnBeforeAccessAsync(args).ConfigureAwait(continueOnCapturedContext: false);
				await ((ITokenCacheInternal)this).OnBeforeWriteAsync(args).ConfigureAwait(continueOnCapturedContext: false);
				if (msalAccessTokenCacheItem != null)
				{
					requestParams.RequestContext.Logger.Info("Saving AT in cache and removing overlapping ATs...");
					DeleteAccessTokensWithIntersectingScopes(requestParams, instanceDiscoveryMetadata.Aliases, tenantId, msalAccessTokenCacheItem.ScopeSet, msalAccessTokenCacheItem.HomeAccountId, msalAccessTokenCacheItem.TokenType);
					_accessor.SaveAccessToken(msalAccessTokenCacheItem);
				}
				if (idToken != null)
				{
					requestParams.RequestContext.Logger.Info("Saving Id Token and Account in cache ...");
					_accessor.SaveIdToken(msalIdTokenCacheItem);
					MergeWamAccountIds(msalAccountCacheItem);
					_accessor.SaveAccount(msalAccountCacheItem);
				}
				if (msalRefreshTokenCacheItem != null)
				{
					requestParams.RequestContext.Logger.Info("Saving RT in cache...");
					_accessor.SaveRefreshToken(msalRefreshTokenCacheItem);
				}
				UpdateAppMetadata(requestParams.ClientId, instanceDiscoveryMetadata.PreferredCache, response.FamilyId);
				if (!requestParams.IsClientCredentialRequest && requestParams.AuthorityInfo.AuthorityType != AuthorityType.B2C)
				{
					Authority authority = Authority.CreateAuthorityWithEnvironment(requestParams.TenantUpdatedCanonicalAuthority.AuthorityInfo, instanceDiscoveryMetadata.PreferredCache);
					CacheFallbackOperations.WriteAdalRefreshToken(Logger, LegacyCachePersistence, msalRefreshTokenCacheItem, msalIdTokenCacheItem, authority.AuthorityInfo.CanonicalAuthority, msalIdTokenCacheItem.IdToken.ObjectId, response.Scope);
				}
			}
			finally
			{
				TokenCacheNotificationArgs args2 = new TokenCacheNotificationArgs(this, ClientId, account, hasStateChanged: true, ((ITokenCacheInternal)this).IsApplicationCache, ((ITokenCacheInternal)this).HasTokensNoLocks(), suggestedWebCacheKey);
				await ((ITokenCacheInternal)this).OnAfterAccessAsync(args2).ConfigureAwait(continueOnCapturedContext: false);
				HasStateChanged = false;
			}
			return Tuple.Create(msalAccessTokenCacheItem, msalIdTokenCacheItem);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	private void MergeWamAccountIds(MsalAccountCacheItem msalAccountCacheItem)
	{
		IDictionary<string, string> other = _accessor.GetAllAccounts().SingleOrDefault((MsalAccountCacheItem acc) => string.Equals(acc.GetKey().ToString(), msalAccountCacheItem.GetKey().ToString(), StringComparison.OrdinalIgnoreCase))?.WamAccountIds;
		msalAccountCacheItem.WamAccountIds.MergeDifferentEntries(other);
	}

	private static Dictionary<string, string> GetWamAccountIds(AuthenticationRequestParameters requestParams, MsalTokenResponse response)
	{
		if (!string.IsNullOrEmpty(response.WamAccountId))
		{
			return new Dictionary<string, string> { { requestParams.ClientId, response.WamAccountId } };
		}
		return new Dictionary<string, string>();
	}

	private static string GetHomeAccountId(AuthenticationRequestParameters requestParams, MsalTokenResponse response, IdToken idToken)
	{
		string text = idToken?.Subject;
		if (idToken != null && idToken.Subject != null)
		{
			requestParams.RequestContext.Logger.Info("Subject not present in Id token");
		}
		return ((response.ClientInfo != null) ? ClientInfo.CreateFromJson(response.ClientInfo) : null)?.ToAccountIdentifier() ?? text;
	}

	private static string GetPreferredUsernameFromIdToken(bool isAdfsAuthority, IdToken idToken)
	{
		if (idToken == null)
		{
			return "Missing from the token response";
		}
		if (string.IsNullOrWhiteSpace(idToken.PreferredUsername))
		{
			if (isAdfsAuthority)
			{
				if (string.IsNullOrEmpty(idToken.Upn))
				{
					return "Missing from the token response";
				}
				return idToken.Upn;
			}
			return "Missing from the token response";
		}
		return idToken.PreferredUsername;
	}

	async Task<MsalAccessTokenCacheItem> ITokenCacheInternal.FindAccessTokenAsync(AuthenticationRequestParameters requestParams)
	{
		ICoreLogger logger = requestParams.RequestContext.Logger;
		if (requestParams.AuthorityInfo?.CanonicalAuthority == null)
		{
			logger.Warning("No authority provided. Skipping cache lookup ");
			return null;
		}
		logger.Info("Looking up access token in the cache.");
		IEnumerable<MsalAccessTokenCacheItem> allAccessTokensWithNoLocks = GetAllAccessTokensWithNoLocks(filterByClientId: true);
		allAccessTokensWithNoLocks = FilterByHomeAccountTenantOrAssertion(requestParams, allAccessTokensWithNoLocks);
		allAccessTokensWithNoLocks = FilterByTokenType(requestParams, allAccessTokensWithNoLocks);
		if (!allAccessTokensWithNoLocks.Any())
		{
			logger.Info("No matching entry found for user or assertion");
			return null;
		}
		if (logger.IsLoggingEnabled(LogLevel.Info))
		{
			logger.Info("Matching entry count - " + allAccessTokensWithNoLocks.Count());
		}
		allAccessTokensWithNoLocks = FilterByScopes(requestParams, allAccessTokensWithNoLocks);
		allAccessTokensWithNoLocks = await FilterByEnvironmentAsync(requestParams, allAccessTokensWithNoLocks).ConfigureAwait(continueOnCapturedContext: false);
		if (!allAccessTokensWithNoLocks.Any())
		{
			logger.Info("No tokens found for matching authority, client_id, user and scopes.");
			return null;
		}
		MsalAccessTokenCacheItem singleResult = GetSingleResult(requestParams, allAccessTokensWithNoLocks);
		singleResult = FilterByKeyId(singleResult, requestParams);
		return GetUnexpiredAccessTokenOrNull(requestParams, singleResult);
	}

	private static IEnumerable<MsalAccessTokenCacheItem> FilterByScopes(AuthenticationRequestParameters requestParams, IEnumerable<MsalAccessTokenCacheItem> tokenCacheItems)
	{
		IEnumerable<string> requestScopes = requestParams.Scope.Where((string s) => !OAuth2Value.ReservedScopes.Contains(s));
		tokenCacheItems = tokenCacheItems.FilterWithLogging((MsalAccessTokenCacheItem item) => ScopeHelper.ScopeContains(item.ScopeSet, requestScopes), requestParams.RequestContext.Logger, "Filtering by scopes");
		return tokenCacheItems;
	}

	private static IEnumerable<MsalAccessTokenCacheItem> FilterByTokenType(AuthenticationRequestParameters requestParams, IEnumerable<MsalAccessTokenCacheItem> tokenCacheItems)
	{
		tokenCacheItems = tokenCacheItems.FilterWithLogging((MsalAccessTokenCacheItem item) => string.Equals(item.TokenType ?? "bearer", requestParams.AuthenticationScheme.AccessTokenType, StringComparison.OrdinalIgnoreCase), requestParams.RequestContext.Logger, "Filtering by token type");
		return tokenCacheItems;
	}

	private static IEnumerable<MsalAccessTokenCacheItem> FilterByHomeAccountTenantOrAssertion(AuthenticationRequestParameters requestParams, IEnumerable<MsalAccessTokenCacheItem> tokenCacheItems)
	{
		string requestTenantId = requestParams.Authority.TenantId;
		bool flag = true;
		if (requestParams.UserAssertion != null)
		{
			tokenCacheItems = tokenCacheItems.FilterWithLogging((MsalAccessTokenCacheItem item) => !string.IsNullOrEmpty(item.UserAssertionHash) && item.UserAssertionHash.Equals(requestParams.UserAssertion.AssertionHash, StringComparison.OrdinalIgnoreCase), requestParams.RequestContext.Logger, "Filtering by user assertion id");
			flag = !string.IsNullOrEmpty(requestTenantId) && !AadAuthority.IsCommonOrganizationsOrConsumersTenant(requestTenantId);
		}
		if (flag)
		{
			tokenCacheItems = tokenCacheItems.FilterWithLogging((MsalAccessTokenCacheItem item) => string.Equals(item.TenantId ?? string.Empty, requestTenantId ?? string.Empty, StringComparison.OrdinalIgnoreCase), requestParams.RequestContext.Logger, "Filtering by tenant id");
		}
		else
		{
			requestParams.RequestContext.Logger.Warning("Have not filtered by tenant ID. This can happen in OBO scenario where authority is /common or /organizations. Please use tenanted authority.");
		}
		if (requestParams.ApiId != ApiEvent.ApiIds.AcquireTokenForClient && requestParams.ApiId != ApiEvent.ApiIds.AcquireTokenOnBehalfOf)
		{
			tokenCacheItems = tokenCacheItems.FilterWithLogging((MsalAccessTokenCacheItem item) => item.HomeAccountId.Equals(requestParams.Account.HomeAccountId?.Identifier, StringComparison.OrdinalIgnoreCase), requestParams.RequestContext.Logger, "Filtering by home account id");
		}
		return tokenCacheItems;
	}

	private MsalAccessTokenCacheItem GetUnexpiredAccessTokenOrNull(AuthenticationRequestParameters requestParams, MsalAccessTokenCacheItem msalAccessTokenCacheItem)
	{
		ICoreLogger logger = requestParams.RequestContext.Logger;
		if (msalAccessTokenCacheItem != null)
		{
			if (msalAccessTokenCacheItem.ExpiresOn > DateTime.UtcNow + AccessTokenExpirationBuffer)
			{
				if (msalAccessTokenCacheItem.ExpiresOn > DateTime.UtcNow + TimeSpan.FromDays(3650.0))
				{
					logger.Error("Access token expiration too large. This can be the result of a bug or corrupt cache. Token will be ignored as it is likely expired." + GetAccessTokenExpireLogMessageContent(msalAccessTokenCacheItem));
					return null;
				}
				if (logger.IsLoggingEnabled(LogLevel.Info))
				{
					logger.Info("Access token is not expired. Returning the found cache entry. " + GetAccessTokenExpireLogMessageContent(msalAccessTokenCacheItem));
				}
				return msalAccessTokenCacheItem;
			}
			if (ServiceBundle.Config.IsExtendedTokenLifetimeEnabled && msalAccessTokenCacheItem.ExtendedExpiresOn > DateTime.UtcNow + AccessTokenExpirationBuffer)
			{
				if (logger.IsLoggingEnabled(LogLevel.Info))
				{
					logger.Info("Access token is expired.  IsExtendedLifeTimeEnabled=TRUE and ExtendedExpiresOn is not exceeded.  Returning the found cache entry. " + GetAccessTokenExpireLogMessageContent(msalAccessTokenCacheItem));
				}
				msalAccessTokenCacheItem.IsExtendedLifeTimeToken = true;
				return msalAccessTokenCacheItem;
			}
			if (logger.IsLoggingEnabled(LogLevel.Info))
			{
				logger.Info("Access token has expired or about to expire. " + GetAccessTokenExpireLogMessageContent(msalAccessTokenCacheItem));
			}
		}
		return null;
	}

	private static MsalAccessTokenCacheItem GetSingleResult(AuthenticationRequestParameters requestParams, IEnumerable<MsalAccessTokenCacheItem> filteredItems)
	{
		if (filteredItems.Count() == 1)
		{
			return filteredItems.First();
		}
		requestParams.RequestContext.Logger.Error("Multiple tokens found for matching authority, client_id, user and scopes.");
		throw new MsalClientException("multiple_matching_tokens_detected", "The cache contains multiple tokens satisfying the requirements. Try to clear token cache");
	}

	private async Task<IEnumerable<MsalAccessTokenCacheItem>> FilterByEnvironmentAsync(AuthenticationRequestParameters requestParams, IEnumerable<MsalAccessTokenCacheItem> filteredItems)
	{
		InstanceDiscoveryMetadataEntry instanceMetadata = await ServiceBundle.InstanceDiscoveryManager.GetMetadataEntryTryAvoidNetworkAsync(requestParams.AuthorityInfo.CanonicalAuthority, filteredItems.Select((MsalAccessTokenCacheItem at) => at.Environment), requestParams.RequestContext).ConfigureAwait(continueOnCapturedContext: false);
		IEnumerable<MsalAccessTokenCacheItem> enumerable = filteredItems.Where((MsalAccessTokenCacheItem at) => at.Environment.Equals(instanceMetadata.PreferredCache, StringComparison.OrdinalIgnoreCase));
		if (enumerable.Any())
		{
			return enumerable;
		}
		return filteredItems.Where((MsalAccessTokenCacheItem item) => instanceMetadata.Aliases.ContainsOrdinalIgnoreCase(item.Environment));
	}

	private MsalAccessTokenCacheItem FilterByKeyId(MsalAccessTokenCacheItem item, AuthenticationRequestParameters authenticationRequest)
	{
		if (item == null)
		{
			return null;
		}
		string keyId = authenticationRequest.AuthenticationScheme.KeyId;
		if (string.IsNullOrEmpty(item.KeyId) && string.IsNullOrEmpty(keyId))
		{
			authenticationRequest.RequestContext.Logger.Verbose("Bearer token found");
			return item;
		}
		if (string.Equals(item.KeyId, keyId, StringComparison.OrdinalIgnoreCase))
		{
			authenticationRequest.RequestContext.Logger.Verbose("Keyed token found");
			return item;
		}
		authenticationRequest.RequestContext.Logger.Info(string.Format(CultureInfo.InvariantCulture, "A token bound to the wrong key was found. Token key id: {0} Request key id: {1}", item.KeyId, keyId));
		return null;
	}

	async Task<MsalRefreshTokenCacheItem> ITokenCacheInternal.FindRefreshTokenAsync(AuthenticationRequestParameters requestParams, string familyId)
	{
		if (requestParams.Authority == null)
		{
			return null;
		}
		IEnumerable<MsalRefreshTokenCacheItem> allRts = _accessor.GetAllRefreshTokens();
		string[] aliases = (await ServiceBundle.InstanceDiscoveryManager.GetMetadataEntryTryAvoidNetworkAsync(requestParams.AuthorityInfo.CanonicalAuthority, allRts.Select((MsalRefreshTokenCacheItem rt) => rt.Environment), requestParams.RequestContext).ConfigureAwait(continueOnCapturedContext: false)).Aliases;
		IEnumerable<MsalRefreshTokenCacheKey> candidateRtKeys = aliases.Select((string al) => new MsalRefreshTokenCacheKey(al, requestParams.ClientId, requestParams.Account?.HomeAccountId?.Identifier, familyId));
		MsalRefreshTokenCacheItem msalRefreshTokenCacheItem = allRts.FirstOrDefault((MsalRefreshTokenCacheItem rt) => candidateRtKeys.Any((MsalRefreshTokenCacheKey candidateKey) => object.Equals(rt.GetKey(), candidateKey)));
		requestParams.RequestContext.Logger.Info("Refresh token found in the cache? - " + (msalRefreshTokenCacheItem != null));
		if (msalRefreshTokenCacheItem != null)
		{
			return msalRefreshTokenCacheItem;
		}
		requestParams.RequestContext.Logger.Info("Checking ADAL cache for matching RT");
		if (requestParams.Account != null && string.IsNullOrEmpty(familyId))
		{
			return CacheFallbackOperations.GetRefreshToken(Logger, LegacyCachePersistence, aliases, requestParams.ClientId, requestParams.Account);
		}
		return null;
	}

	async Task<bool?> ITokenCacheInternal.IsFociMemberAsync(AuthenticationRequestParameters requestParams, string familyId)
	{
		ICoreLogger logger = requestParams.RequestContext.Logger;
		if (requestParams?.AuthorityInfo?.CanonicalAuthority == null)
		{
			logger.Warning("No authority details, can't check app metadata. Returning unknown");
			return null;
		}
		IEnumerable<MsalAppMetadataCacheItem> allAppMetadata = _accessor.GetAllAppMetadata();
		MsalAppMetadataCacheItem msalAppMetadataCacheItem = (await ServiceBundle.InstanceDiscoveryManager.GetMetadataEntryTryAvoidNetworkAsync(requestParams.AuthorityInfo.CanonicalAuthority, allAppMetadata.Select((MsalAppMetadataCacheItem m) => m.Environment), requestParams.RequestContext).ConfigureAwait(continueOnCapturedContext: false)).Aliases.Select((string env) => _accessor.GetAppMetadata(new MsalAppMetadataCacheKey(ClientId, env))).FirstOrDefault((MsalAppMetadataCacheItem item) => item != null);
		if (msalAppMetadataCacheItem == null)
		{
			logger.Warning("No app metadata found. Returning unknown");
			return null;
		}
		return msalAppMetadataCacheItem.FamilyId == familyId;
	}

	MsalIdTokenCacheItem ITokenCacheInternal.GetIdTokenCacheItem(MsalIdTokenCacheKey msalIdTokenCacheKey)
	{
		return _accessor.GetIdToken(msalIdTokenCacheKey);
	}

	async Task<IEnumerable<IAccount>> ITokenCacheInternal.GetAccountsAsync(AuthenticationRequestParameters requestParameters)
	{
		ICoreLogger logger = requestParameters.RequestContext.Logger;
		string environment = Authority.GetEnviroment(requestParameters.AuthorityInfo.CanonicalAuthority);
		bool filterByClientId = !_featureFlags.IsFociEnabled;
		IEnumerable<MsalRefreshTokenCacheItem> rtCacheItems2 = GetAllRefreshTokensWithNoLocks(filterByClientId);
		IEnumerable<MsalAccountCacheItem> accountCacheItems2 = _accessor.GetAllAccounts();
		if (logger.IsLoggingEnabled(LogLevel.Verbose))
		{
			logger.Verbose($"GetAccounts found {rtCacheItems2.Count()} RTs and {accountCacheItems2.Count()} accounts in MSAL cache.");
		}
		AdalUsersForMsal adalUsersResult = CacheFallbackOperations.GetAllAdalUsersForMsal(Logger, LegacyCachePersistence, ClientId);
		ISet<string> set = new HashSet<string>(accountCacheItems2.Select((MsalAccountCacheItem aci) => aci.Environment), StringComparer.OrdinalIgnoreCase);
		set.UnionWith(rtCacheItems2.Select((MsalRefreshTokenCacheItem rt) => rt.Environment));
		set.UnionWith(adalUsersResult.GetAdalUserEnviroments());
		InstanceDiscoveryMetadataEntry instanceMetadata = await ServiceBundle.InstanceDiscoveryManager.GetMetadataEntryTryAvoidNetworkAsync(requestParameters.AuthorityInfo.CanonicalAuthority, set, requestParameters.RequestContext).ConfigureAwait(continueOnCapturedContext: false);
		rtCacheItems2 = rtCacheItems2.Where((MsalRefreshTokenCacheItem rt) => instanceMetadata.Aliases.ContainsOrdinalIgnoreCase(rt.Environment));
		accountCacheItems2 = accountCacheItems2.Where((MsalAccountCacheItem acc) => instanceMetadata.Aliases.ContainsOrdinalIgnoreCase(acc.Environment));
		if (logger.IsLoggingEnabled(LogLevel.Verbose))
		{
			logger.Verbose($"GetAccounts found {rtCacheItems2.Count()} RTs and {accountCacheItems2.Count()} accounts in MSAL cache after environment filtering.");
		}
		IDictionary<string, Account> dictionary = new Dictionary<string, Account>();
		foreach (MsalRefreshTokenCacheItem item in rtCacheItems2)
		{
			foreach (MsalAccountCacheItem item2 in accountCacheItems2)
			{
				if (RtMatchesAccount(item, item2))
				{
					dictionary[item.HomeAccountId] = new Account(item2.HomeAccountId, item2.PreferredUsername, environment);
					break;
				}
			}
		}
		UpdateMapWithAdalAccountsWithClientInfo(environment, instanceMetadata.Aliases, adalUsersResult, dictionary);
		if (requestParameters.IsBrokerConfigured && ServiceBundle.PlatformProxy.BrokerSupportsWamAccounts)
		{
			foreach (MsalAccountCacheItem item3 in accountCacheItems2.Where((MsalAccountCacheItem acc) => acc.WamAccountIds != null && acc.WamAccountIds.ContainsKey(requestParameters.ClientId)))
			{
				Account value = new Account(item3.HomeAccountId, item3.PreferredUsername, environment, item3.WamAccountIds);
				dictionary[item3.HomeAccountId] = value;
			}
		}
		IEnumerable<IAccount> enumerable = UpdateWithAdalAccountsWithoutClientInfo(environment, instanceMetadata.Aliases, adalUsersResult, dictionary);
		if (!string.IsNullOrEmpty(requestParameters.HomeAccountId))
		{
			enumerable = enumerable.Where((IAccount acc) => acc.HomeAccountId.Identifier.Equals(requestParameters.HomeAccountId, StringComparison.OrdinalIgnoreCase));
			if (logger.IsLoggingEnabled(LogLevel.Verbose))
			{
				logger.Verbose($"Filtered by home account id. Remaining accounts {enumerable.Count()} ");
			}
		}
		return enumerable;
	}

	async Task<IEnumerable<MsalRefreshTokenCacheItem>> ITokenCacheInternal.GetAllRefreshTokensAsync(bool filterByClientId)
	{
		await _semaphoreSlim.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			return GetAllRefreshTokensWithNoLocks(filterByClientId);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	async Task<IEnumerable<MsalAccessTokenCacheItem>> ITokenCacheInternal.GetAllAccessTokensAsync(bool filterByClientId)
	{
		await _semaphoreSlim.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			return GetAllAccessTokensWithNoLocks(filterByClientId);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	async Task<IEnumerable<MsalIdTokenCacheItem>> ITokenCacheInternal.GetAllIdTokensAsync(bool filterByClientId)
	{
		await _semaphoreSlim.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			return GetAllIdTokensWithNoLocks(filterByClientId);
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	async Task<IEnumerable<MsalAccountCacheItem>> ITokenCacheInternal.GetAllAccountsAsync()
	{
		await _semaphoreSlim.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			return _accessor.GetAllAccounts();
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	async Task ITokenCacheInternal.RemoveAccountAsync(IAccount account, RequestContext requestContext)
	{
		await _semaphoreSlim.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			requestContext.Logger.Info("Removing user from cache..");
			try
			{
				TokenCacheNotificationArgs args = new TokenCacheNotificationArgs(this, ClientId, account, hasStateChanged: true, ((ITokenCacheInternal)this).IsApplicationCache, ((ITokenCacheInternal)this).HasTokensNoLocks(), account.HomeAccountId.Identifier);
				try
				{
					await ((ITokenCacheInternal)this).OnBeforeAccessAsync(args).ConfigureAwait(continueOnCapturedContext: false);
					await ((ITokenCacheInternal)this).OnBeforeWriteAsync(args).ConfigureAwait(continueOnCapturedContext: false);
					((ITokenCacheInternal)this).RemoveMsalAccountWithNoLocks(account, requestContext);
					RemoveAdalUser(account);
				}
				finally
				{
					TokenCacheNotificationArgs args2 = new TokenCacheNotificationArgs(this, ClientId, account, hasStateChanged: true, ((ITokenCacheInternal)this).IsApplicationCache, ((ITokenCacheInternal)this).HasTokensNoLocks(), account.HomeAccountId.Identifier);
					await ((ITokenCacheInternal)this).OnAfterAccessAsync(args2).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			finally
			{
				HasStateChanged = false;
			}
		}
		finally
		{
			_semaphoreSlim.Release();
		}
	}

	bool ITokenCacheInternal.HasTokensNoLocks()
	{
		if (!_accessor.GetAllRefreshTokens().Any())
		{
			return _accessor.GetAllAccessTokens().Any((MsalAccessTokenCacheItem at) => !IsAtExpired(at));
		}
		return true;
	}

	private bool IsAtExpired(MsalAccessTokenCacheItem at)
	{
		return at.ExpiresOn < DateTime.UtcNow + AccessTokenExpirationBuffer;
	}

	void ITokenCacheInternal.RemoveMsalAccountWithNoLocks(IAccount account, RequestContext requestContext)
	{
		if (account.HomeAccountId == null)
		{
			return;
		}
		List<MsalRefreshTokenCacheItem> list = (from item in GetAllRefreshTokensWithNoLocks(filterByClientId: false)
			where item.HomeAccountId.Equals(account.HomeAccountId.Identifier, StringComparison.OrdinalIgnoreCase)
			select item).ToList();
		bool flag = !_featureFlags.IsFociEnabled || !FrtExists(list);
		IEnumerable<MsalRefreshTokenCacheItem> enumerable2;
		if (!flag)
		{
			IEnumerable<MsalRefreshTokenCacheItem> enumerable = list;
			enumerable2 = enumerable;
		}
		else
		{
			enumerable2 = list.Where((MsalRefreshTokenCacheItem x) => x.ClientId.Equals(ClientId, StringComparison.OrdinalIgnoreCase));
		}
		foreach (MsalRefreshTokenCacheItem item in enumerable2)
		{
			_accessor.DeleteRefreshToken(item.GetKey());
		}
		requestContext.Logger.Info("Deleted refresh token count - " + list.Count);
		IList<MsalAccessTokenCacheItem> list2 = (from item in GetAllAccessTokensWithNoLocks(flag)
			where item.HomeAccountId.Equals(account.HomeAccountId.Identifier, StringComparison.OrdinalIgnoreCase)
			select item).ToList();
		foreach (MsalAccessTokenCacheItem item2 in list2)
		{
			_accessor.DeleteAccessToken(item2.GetKey());
		}
		requestContext.Logger.Info("Deleted access token count - " + list2.Count);
		List<MsalIdTokenCacheItem> list3 = (from item in GetAllIdTokensWithNoLocks(flag)
			where item.HomeAccountId.Equals(account.HomeAccountId.Identifier, StringComparison.OrdinalIgnoreCase)
			select item).ToList();
		foreach (MsalIdTokenCacheItem item3 in list3)
		{
			_accessor.DeleteIdToken(item3.GetKey());
		}
		requestContext.Logger.Info("Deleted Id token count - " + list3.Count);
		(from item in _accessor.GetAllAccounts()
			where item.HomeAccountId.Equals(account.HomeAccountId.Identifier, StringComparison.OrdinalIgnoreCase) && item.PreferredUsername.Equals(account.Username, StringComparison.OrdinalIgnoreCase)
			select item).ToList().ForEach(delegate(MsalAccountCacheItem accItem)
		{
			_accessor.DeleteAccount(accItem.GetKey());
		});
	}

	async Task ITokenCacheInternal.OnAfterAccessAsync(TokenCacheNotificationArgs args)
	{
		AfterAccess?.Invoke(args);
		if (AsyncAfterAccess != null)
		{
			await AsyncAfterAccess(args).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	async Task ITokenCacheInternal.OnBeforeAccessAsync(TokenCacheNotificationArgs args)
	{
		BeforeAccess?.Invoke(args);
		if (AsyncBeforeAccess != null)
		{
			await AsyncBeforeAccess(args).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	async Task ITokenCacheInternal.OnBeforeWriteAsync(TokenCacheNotificationArgs args)
	{
		HasStateChanged = true;
		args.HasStateChanged = true;
		BeforeWrite?.Invoke(args);
		if (AsyncBeforeWrite != null)
		{
			await AsyncBeforeWrite(args).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public void SetBeforeAccess(TokenCacheCallback beforeAccess)
	{
		GuardOnMobilePlatforms();
		BeforeAccess = beforeAccess;
	}

	public void SetAfterAccess(TokenCacheCallback afterAccess)
	{
		GuardOnMobilePlatforms();
		AfterAccess = afterAccess;
	}

	public void SetBeforeWrite(TokenCacheCallback beforeWrite)
	{
		GuardOnMobilePlatforms();
		BeforeWrite = beforeWrite;
	}

	public void SetBeforeAccessAsync(Func<TokenCacheNotificationArgs, Task> beforeAccess)
	{
		GuardOnMobilePlatforms();
		AsyncBeforeAccess = beforeAccess;
	}

	public void SetAfterAccessAsync(Func<TokenCacheNotificationArgs, Task> afterAccess)
	{
		GuardOnMobilePlatforms();
		AsyncAfterAccess = afterAccess;
	}

	public void SetBeforeWriteAsync(Func<TokenCacheNotificationArgs, Task> beforeWrite)
	{
		GuardOnMobilePlatforms();
		AsyncBeforeWrite = beforeWrite;
	}

	private static void GuardOnMobilePlatforms()
	{
	}

	byte[] ITokenCacheSerializer.SerializeAdalV3()
	{
		return LegacyCachePersistence.LoadCache();
	}

	void ITokenCacheSerializer.DeserializeAdalV3(byte[] adalV3State)
	{
		LegacyCachePersistence.WriteCache(adalV3State);
	}

	byte[] ITokenCacheSerializer.SerializeMsalV2()
	{
		return new TokenCacheDictionarySerializer(_accessor).Serialize(_unknownNodes);
	}

	void ITokenCacheSerializer.DeserializeMsalV2(byte[] msalV2State)
	{
		_unknownNodes = new TokenCacheDictionarySerializer(_accessor).Deserialize(msalV2State, clearExistingCacheData: false);
	}

	byte[] ITokenCacheSerializer.SerializeMsalV3()
	{
		return new TokenCacheJsonSerializer(_accessor).Serialize(_unknownNodes);
	}

	void ITokenCacheSerializer.DeserializeMsalV3(byte[] msalV3State, bool shouldClearExistingCache)
	{
		if (msalV3State != null && msalV3State.Length != 0)
		{
			_unknownNodes = new TokenCacheJsonSerializer(_accessor).Deserialize(msalV3State, shouldClearExistingCache);
		}
	}
}
