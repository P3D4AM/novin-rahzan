using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Identity.Client.Cache;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Cache.Keys;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.PlatformsCommon.Shared;

internal class InMemoryTokenCacheAccessor : ITokenCacheAccessor
{
	private readonly ConcurrentDictionary<string, MsalAccessTokenCacheItem> _accessTokenCacheDictionary = new ConcurrentDictionary<string, MsalAccessTokenCacheItem>(1, 1);

	private readonly ConcurrentDictionary<string, MsalRefreshTokenCacheItem> _refreshTokenCacheDictionary = new ConcurrentDictionary<string, MsalRefreshTokenCacheItem>(1, 1);

	private readonly ConcurrentDictionary<string, MsalIdTokenCacheItem> _idTokenCacheDictionary = new ConcurrentDictionary<string, MsalIdTokenCacheItem>(1, 1);

	private readonly ConcurrentDictionary<string, MsalAccountCacheItem> _accountCacheDictionary = new ConcurrentDictionary<string, MsalAccountCacheItem>(1, 1);

	private readonly ConcurrentDictionary<string, MsalAppMetadataCacheItem> _appMetadataDictionary = new ConcurrentDictionary<string, MsalAppMetadataCacheItem>(1, 1);

	private readonly ICoreLogger _logger;

	public InMemoryTokenCacheAccessor(ICoreLogger logger)
	{
		_logger = logger ?? throw new ArgumentNullException("logger");
	}

	public void SaveAccessToken(MsalAccessTokenCacheItem item)
	{
		string key = item.GetKey().ToString();
		_accessTokenCacheDictionary.AddOrUpdate(key, item, (string k, MsalAccessTokenCacheItem oldValue) => item);
	}

	public void SaveRefreshToken(MsalRefreshTokenCacheItem item)
	{
		string key = item.GetKey().ToString();
		_refreshTokenCacheDictionary.AddOrUpdate(key, item, (string k, MsalRefreshTokenCacheItem oldValue) => item);
	}

	public void SaveIdToken(MsalIdTokenCacheItem item)
	{
		string key = item.GetKey().ToString();
		_idTokenCacheDictionary.AddOrUpdate(key, item, (string k, MsalIdTokenCacheItem oldValue) => item);
	}

	public void SaveAccount(MsalAccountCacheItem item)
	{
		string key = item.GetKey().ToString();
		_accountCacheDictionary.AddOrUpdate(key, item, (string k, MsalAccountCacheItem oldValue) => item);
	}

	public void SaveAppMetadata(MsalAppMetadataCacheItem item)
	{
		string key = item.GetKey().ToString();
		_appMetadataDictionary.AddOrUpdate(key, item, (string k, MsalAppMetadataCacheItem oldValue) => item);
	}

	public MsalAccessTokenCacheItem GetAccessToken(MsalAccessTokenCacheKey accessTokenKey)
	{
		_accessTokenCacheDictionary.TryGetValue(accessTokenKey.ToString(), out var value);
		return value;
	}

	public MsalRefreshTokenCacheItem GetRefreshToken(MsalRefreshTokenCacheKey refreshTokenKey)
	{
		_refreshTokenCacheDictionary.TryGetValue(refreshTokenKey.ToString(), out var value);
		return value;
	}

	public MsalIdTokenCacheItem GetIdToken(MsalIdTokenCacheKey idTokenKey)
	{
		_idTokenCacheDictionary.TryGetValue(idTokenKey.ToString(), out var value);
		return value;
	}

	public MsalAccountCacheItem GetAccount(MsalAccountCacheKey accountKey)
	{
		_accountCacheDictionary.TryGetValue(accountKey.ToString(), out var value);
		return value;
	}

	public MsalAppMetadataCacheItem GetAppMetadata(MsalAppMetadataCacheKey appMetadataKey)
	{
		_appMetadataDictionary.TryGetValue(appMetadataKey.ToString(), out var value);
		return value;
	}

	public void DeleteAccessToken(MsalAccessTokenCacheKey cacheKey)
	{
		if (!_accessTokenCacheDictionary.TryRemove(cacheKey.ToString(), out var _))
		{
			_logger.InfoPii($"Cannot delete an access token because it was already deleted. Key {cacheKey}", "Cannot delete an access token because it was already deleted");
		}
	}

	public void DeleteRefreshToken(MsalRefreshTokenCacheKey cacheKey)
	{
		if (!_refreshTokenCacheDictionary.TryRemove(cacheKey.ToString(), out var _))
		{
			_logger.InfoPii($"Cannot delete an refresh token because it was already deleted. Key {cacheKey}", "Cannot delete an refresh token because it was already deleted");
		}
	}

	public void DeleteIdToken(MsalIdTokenCacheKey cacheKey)
	{
		if (!_idTokenCacheDictionary.TryRemove(cacheKey.ToString(), out var _))
		{
			_logger.InfoPii($"Cannot delete an id token because it was already deleted. Key {cacheKey}", "Cannot delete an id token because it was already deleted");
		}
	}

	public void DeleteAccount(MsalAccountCacheKey cacheKey)
	{
		if (!_accountCacheDictionary.TryRemove(cacheKey.ToString(), out var _))
		{
			_logger.InfoPii($"Cannot delete an account because it was already deleted. Key {cacheKey}", "Cannot delete an account because it was already deleted");
		}
	}

	public IEnumerable<MsalAccessTokenCacheItem> GetAllAccessTokens()
	{
		List<MsalAccessTokenCacheItem> list = new List<MsalAccessTokenCacheItem>();
		foreach (KeyValuePair<string, MsalAccessTokenCacheItem> item in _accessTokenCacheDictionary)
		{
			list.Add(item.Value);
		}
		return list;
	}

	public IEnumerable<MsalRefreshTokenCacheItem> GetAllRefreshTokens()
	{
		List<MsalRefreshTokenCacheItem> list = new List<MsalRefreshTokenCacheItem>();
		foreach (KeyValuePair<string, MsalRefreshTokenCacheItem> item in _refreshTokenCacheDictionary)
		{
			list.Add(item.Value);
		}
		return list;
	}

	public IEnumerable<MsalIdTokenCacheItem> GetAllIdTokens()
	{
		List<MsalIdTokenCacheItem> list = new List<MsalIdTokenCacheItem>();
		foreach (KeyValuePair<string, MsalIdTokenCacheItem> item in _idTokenCacheDictionary)
		{
			list.Add(item.Value);
		}
		return list;
	}

	public IEnumerable<MsalAccountCacheItem> GetAllAccounts()
	{
		List<MsalAccountCacheItem> list = new List<MsalAccountCacheItem>();
		foreach (KeyValuePair<string, MsalAccountCacheItem> item in _accountCacheDictionary)
		{
			list.Add(item.Value);
		}
		return list;
	}

	public IEnumerable<MsalAppMetadataCacheItem> GetAllAppMetadata()
	{
		List<MsalAppMetadataCacheItem> list = new List<MsalAppMetadataCacheItem>();
		foreach (KeyValuePair<string, MsalAppMetadataCacheItem> item in _appMetadataDictionary)
		{
			list.Add(item.Value);
		}
		return list;
	}

	public void SetiOSKeychainSecurityGroup(string keychainSecurityGroup)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		_accessTokenCacheDictionary.Clear();
		_refreshTokenCacheDictionary.Clear();
		_idTokenCacheDictionary.Clear();
		_accountCacheDictionary.Clear();
	}
}
