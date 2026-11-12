using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache;

internal class TokenCacheJsonSerializer : ITokenCacheSerializable
{
	private readonly ITokenCacheAccessor _accessor;

	public TokenCacheJsonSerializer(ITokenCacheAccessor accessor)
	{
		_accessor = accessor;
	}

	public byte[] Serialize(IDictionary<string, JToken> unkownNodes)
	{
		CacheSerializationContract cacheSerializationContract = new CacheSerializationContract(unkownNodes);
		foreach (MsalAccessTokenCacheItem allAccessToken in _accessor.GetAllAccessTokens())
		{
			cacheSerializationContract.AccessTokens[allAccessToken.GetKey().ToString()] = allAccessToken;
		}
		foreach (MsalRefreshTokenCacheItem allRefreshToken in _accessor.GetAllRefreshTokens())
		{
			cacheSerializationContract.RefreshTokens[allRefreshToken.GetKey().ToString()] = allRefreshToken;
		}
		foreach (MsalIdTokenCacheItem allIdToken in _accessor.GetAllIdTokens())
		{
			cacheSerializationContract.IdTokens[allIdToken.GetKey().ToString()] = allIdToken;
		}
		foreach (MsalAccountCacheItem allAccount in _accessor.GetAllAccounts())
		{
			cacheSerializationContract.Accounts[allAccount.GetKey().ToString()] = allAccount;
		}
		foreach (MsalAppMetadataCacheItem allAppMetadatum in _accessor.GetAllAppMetadata())
		{
			cacheSerializationContract.AppMetadata[allAppMetadatum.GetKey().ToString()] = allAppMetadatum;
		}
		return cacheSerializationContract.ToJsonString().ToByteArray();
	}

	public IDictionary<string, JToken> Deserialize(byte[] bytes, bool clearExistingCacheData)
	{
		CacheSerializationContract cacheSerializationContract;
		try
		{
			cacheSerializationContract = CacheSerializationContract.FromJsonString(CoreHelpers.ByteArrayToString(bytes));
		}
		catch (Exception innerException)
		{
			throw new MsalClientException("json_parse_failed", "MSAL V3 Deserialization failed to parse the cache contents. Is this possibly an earlier format needed for DeserializeMsalV2? (See https://aka.ms/msal-net-3x-cache-breaking-change)", innerException);
		}
		if (clearExistingCacheData)
		{
			_accessor.Clear();
		}
		if (cacheSerializationContract.AccessTokens != null)
		{
			foreach (MsalAccessTokenCacheItem value in cacheSerializationContract.AccessTokens.Values)
			{
				_accessor.SaveAccessToken(value);
			}
		}
		if (cacheSerializationContract.RefreshTokens != null)
		{
			foreach (MsalRefreshTokenCacheItem value2 in cacheSerializationContract.RefreshTokens.Values)
			{
				_accessor.SaveRefreshToken(value2);
			}
		}
		if (cacheSerializationContract.IdTokens != null)
		{
			foreach (MsalIdTokenCacheItem value3 in cacheSerializationContract.IdTokens.Values)
			{
				_accessor.SaveIdToken(value3);
			}
		}
		if (cacheSerializationContract.Accounts != null)
		{
			foreach (MsalAccountCacheItem value4 in cacheSerializationContract.Accounts.Values)
			{
				_accessor.SaveAccount(value4);
			}
		}
		if (cacheSerializationContract.AppMetadata != null)
		{
			foreach (MsalAppMetadataCacheItem value5 in cacheSerializationContract.AppMetadata.Values)
			{
				_accessor.SaveAppMetadata(value5);
			}
		}
		return cacheSerializationContract.UnknownNodes;
	}
}
