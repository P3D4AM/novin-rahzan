using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Json;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Cache.Items;

internal class CacheSerializationContract
{
	private static readonly IEnumerable<string> s_knownPropertyNames = new string[5] { "AccessToken", "RefreshToken", "IdToken", "Account", "AppMetadata" };

	public Dictionary<string, MsalAccessTokenCacheItem> AccessTokens { get; set; } = new Dictionary<string, MsalAccessTokenCacheItem>();


	public Dictionary<string, MsalRefreshTokenCacheItem> RefreshTokens { get; set; } = new Dictionary<string, MsalRefreshTokenCacheItem>();


	public Dictionary<string, MsalIdTokenCacheItem> IdTokens { get; set; } = new Dictionary<string, MsalIdTokenCacheItem>();


	public Dictionary<string, MsalAccountCacheItem> Accounts { get; set; } = new Dictionary<string, MsalAccountCacheItem>();


	public Dictionary<string, MsalAppMetadataCacheItem> AppMetadata { get; set; } = new Dictionary<string, MsalAppMetadataCacheItem>();


	public IDictionary<string, JToken> UnknownNodes { get; }

	public CacheSerializationContract(IDictionary<string, JToken> unkownNodes)
	{
		UnknownNodes = unkownNodes ?? new Dictionary<string, JToken>();
	}

	internal static CacheSerializationContract FromJsonString(string json)
	{
		JObject jObject = JObject.Parse(json);
		CacheSerializationContract cacheSerializationContract = new CacheSerializationContract(ExtractUnkownNodes(jObject));
		if (jObject.ContainsKey("AccessToken"))
		{
			foreach (JToken item in jObject["AccessToken"].Values())
			{
				if (item is JObject j)
				{
					MsalAccessTokenCacheItem msalAccessTokenCacheItem = MsalAccessTokenCacheItem.FromJObject(j);
					cacheSerializationContract.AccessTokens[msalAccessTokenCacheItem.GetKey().ToString()] = msalAccessTokenCacheItem;
				}
			}
		}
		if (jObject.ContainsKey("RefreshToken"))
		{
			foreach (JToken item2 in jObject["RefreshToken"].Values())
			{
				if (item2 is JObject j2)
				{
					MsalRefreshTokenCacheItem msalRefreshTokenCacheItem = MsalRefreshTokenCacheItem.FromJObject(j2);
					cacheSerializationContract.RefreshTokens[msalRefreshTokenCacheItem.GetKey().ToString()] = msalRefreshTokenCacheItem;
				}
			}
		}
		if (jObject.ContainsKey("IdToken"))
		{
			foreach (JToken item3 in jObject["IdToken"].Values())
			{
				if (item3 is JObject j3)
				{
					MsalIdTokenCacheItem msalIdTokenCacheItem = MsalIdTokenCacheItem.FromJObject(j3);
					cacheSerializationContract.IdTokens[msalIdTokenCacheItem.GetKey().ToString()] = msalIdTokenCacheItem;
				}
			}
		}
		if (jObject.ContainsKey("Account"))
		{
			foreach (JToken item4 in jObject["Account"].Values())
			{
				if (item4 is JObject j4)
				{
					MsalAccountCacheItem msalAccountCacheItem = MsalAccountCacheItem.FromJObject(j4);
					cacheSerializationContract.Accounts[msalAccountCacheItem.GetKey().ToString()] = msalAccountCacheItem;
				}
			}
		}
		if (jObject.ContainsKey("AppMetadata"))
		{
			foreach (JToken item5 in jObject["AppMetadata"].Values())
			{
				if (item5 is JObject j5)
				{
					MsalAppMetadataCacheItem msalAppMetadataCacheItem = MsalAppMetadataCacheItem.FromJObject(j5);
					cacheSerializationContract.AppMetadata[msalAppMetadataCacheItem.GetKey().ToString()] = msalAppMetadataCacheItem;
				}
			}
		}
		return cacheSerializationContract;
	}

	private static IDictionary<string, JToken> ExtractUnkownNodes(JObject root)
	{
		return root.Where((KeyValuePair<string, JToken> kvp) => !s_knownPropertyNames.Any((string p) => string.Equals(kvp.Key, p, StringComparison.OrdinalIgnoreCase))).ToDictionary((KeyValuePair<string, JToken> kvp) => kvp.Key, (KeyValuePair<string, JToken> kvp) => kvp.Value);
	}

	internal string ToJsonString()
	{
		JObject jObject = new JObject();
		JObject jObject2 = new JObject();
		foreach (KeyValuePair<string, MsalAccessTokenCacheItem> accessToken in AccessTokens)
		{
			jObject2[accessToken.Key] = accessToken.Value.ToJObject();
		}
		jObject["AccessToken"] = jObject2;
		JObject jObject3 = new JObject();
		foreach (KeyValuePair<string, MsalRefreshTokenCacheItem> refreshToken in RefreshTokens)
		{
			jObject3[refreshToken.Key] = refreshToken.Value.ToJObject();
		}
		jObject["RefreshToken"] = jObject3;
		JObject jObject4 = new JObject();
		foreach (KeyValuePair<string, MsalIdTokenCacheItem> idToken in IdTokens)
		{
			jObject4[idToken.Key] = idToken.Value.ToJObject();
		}
		jObject["IdToken"] = jObject4;
		JObject jObject5 = new JObject();
		foreach (KeyValuePair<string, MsalAccountCacheItem> account in Accounts)
		{
			jObject5[account.Key] = account.Value.ToJObject();
		}
		jObject["Account"] = jObject5;
		JObject jObject6 = new JObject();
		foreach (KeyValuePair<string, MsalAppMetadataCacheItem> appMetadatum in AppMetadata)
		{
			jObject6[appMetadatum.Key] = appMetadatum.Value.ToJObject();
		}
		jObject["AppMetadata"] = jObject6;
		foreach (KeyValuePair<string, JToken> unknownNode in UnknownNodes)
		{
			jObject[unknownNode.Key] = unknownNode.Value;
		}
		return JsonConvert.SerializeObject(jObject, Formatting.None, new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Include
		});
	}
}
