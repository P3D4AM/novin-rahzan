using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache;

internal class TokenCacheDictionarySerializer : ITokenCacheSerializable
{
	private const string AccessTokenKey = "access_tokens";

	private const string RefreshTokenKey = "refresh_tokens";

	private const string IdTokenKey = "id_tokens";

	private const string AccountKey = "accounts";

	private readonly ITokenCacheAccessor _accessor;

	public TokenCacheDictionarySerializer(ITokenCacheAccessor accessor)
	{
		_accessor = accessor;
	}

	public byte[] Serialize(IDictionary<string, JToken> unkownNodes)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		foreach (MsalAccessTokenCacheItem allAccessToken in _accessor.GetAllAccessTokens())
		{
			list.Add(allAccessToken.ToJsonString());
		}
		foreach (MsalRefreshTokenCacheItem allRefreshToken in _accessor.GetAllRefreshTokens())
		{
			list2.Add(allRefreshToken.ToJsonString());
		}
		foreach (MsalIdTokenCacheItem allIdToken in _accessor.GetAllIdTokens())
		{
			list3.Add(allIdToken.ToJsonString());
		}
		foreach (MsalAccountCacheItem allAccount in _accessor.GetAllAccounts())
		{
			list4.Add(allAccount.ToJsonString());
		}
		return JsonHelper.SerializeToJson(new Dictionary<string, IEnumerable<string>>
		{
			["access_tokens"] = list,
			["refresh_tokens"] = list2,
			["id_tokens"] = list3,
			["accounts"] = list4
		}.ToList()).ToByteArray();
	}

	public IDictionary<string, JToken> Deserialize(byte[] bytes, bool clearExistingCacheData)
	{
		List<KeyValuePair<string, IEnumerable<string>>> list;
		try
		{
			list = JsonHelper.DeserializeFromJson<List<KeyValuePair<string, IEnumerable<string>>>>(bytes);
		}
		catch (Exception innerException)
		{
			throw new MsalClientException("json_parse_failed", "MSAL V2 Deserialization failed to parse the cache contents. Is this possibly an earlier format needed for DeserializeMsalV3?  (See https://aka.ms/msal-net-3x-cache-breaking-change)", innerException);
		}
		Dictionary<string, IEnumerable<string>> dictionary = list.ToDictionary((KeyValuePair<string, IEnumerable<string>> x) => x.Key, (KeyValuePair<string, IEnumerable<string>> x) => x.Value);
		if (clearExistingCacheData)
		{
			_accessor.Clear();
		}
		if (list == null || list.Count == 0)
		{
			return null;
		}
		if (dictionary.ContainsKey("access_tokens"))
		{
			foreach (string item in dictionary["access_tokens"])
			{
				_accessor.SaveAccessToken(MsalAccessTokenCacheItem.FromJsonString(item));
			}
		}
		if (dictionary.ContainsKey("refresh_tokens"))
		{
			foreach (string item2 in dictionary["refresh_tokens"])
			{
				_accessor.SaveRefreshToken(MsalRefreshTokenCacheItem.FromJsonString(item2));
			}
		}
		if (dictionary.ContainsKey("id_tokens"))
		{
			foreach (string item3 in dictionary["id_tokens"])
			{
				_accessor.SaveIdToken(MsalIdTokenCacheItem.FromJsonString(item3));
			}
		}
		if (dictionary.ContainsKey("accounts"))
		{
			foreach (string item4 in dictionary["accounts"])
			{
				_accessor.SaveAccount(MsalAccountCacheItem.FromJsonString(item4));
			}
		}
		return null;
	}
}
