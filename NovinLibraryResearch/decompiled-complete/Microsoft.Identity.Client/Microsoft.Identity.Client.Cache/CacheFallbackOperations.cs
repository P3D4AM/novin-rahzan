using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Cache;

internal static class CacheFallbackOperations
{
	internal const string DifferentEnvError = "Not expecting the RT and IdT to have different env when adding to legacy cache";

	internal const string DifferentAuthorityError = "Not expecting authority to have a different env than the RT and IdT";

	public static void WriteAdalRefreshToken(ICoreLogger logger, ILegacyCachePersistence legacyCachePersistence, MsalRefreshTokenCacheItem rtItem, MsalIdTokenCacheItem idItem, string authority, string uniqueId, string scope)
	{
		try
		{
			if (rtItem == null)
			{
				logger.Info("No refresh token available. Skipping writing to ADAL legacy cache.");
				return;
			}
			if (!string.IsNullOrEmpty(rtItem.FamilyId))
			{
				logger.Info("Not writing FRT in ADAL legacy cache");
				return;
			}
			AdalTokenCacheKey key = new AdalTokenCacheKey(authority, scope, rtItem.ClientId, TokenSubjectType.User, uniqueId, idItem.IdToken.PreferredUsername);
			AdalResultWrapper value = new AdalResultWrapper
			{
				Result = new AdalResult
				{
					UserInfo = new AdalUserInfo
					{
						UniqueId = uniqueId,
						DisplayableId = idItem.IdToken.PreferredUsername
					}
				},
				RefreshToken = rtItem.Secret,
				RawClientInfo = rtItem.RawClientInfo,
				ResourceInResponse = scope
			};
			IDictionary<AdalTokenCacheKey, AdalResultWrapper> dictionary = AdalCacheOperations.Deserialize(logger, legacyCachePersistence.LoadCache());
			dictionary[key] = value;
			legacyCachePersistence.WriteCache(AdalCacheOperations.Serialize(logger, dictionary));
		}
		catch (Exception exWithPii)
		{
			if (!string.Equals(rtItem?.Environment, idItem?.Environment, StringComparison.OrdinalIgnoreCase))
			{
				logger.Error("Not expecting the RT and IdT to have different env when adding to legacy cache");
			}
			if (!string.Equals(rtItem?.Environment, new Uri(authority).Host, StringComparison.OrdinalIgnoreCase))
			{
				logger.Error("Not expecting authority to have a different env than the RT and IdT");
			}
			logger.WarningPiiWithPrefix(exWithPii, "An error occurred while writing MSAL refresh token to the cache in ADAL format. For details please see https://aka.ms/net-cache-persistence-errors. ");
		}
	}

	public static AdalUsersForMsal GetAllAdalUsersForMsal(ICoreLogger logger, ILegacyCachePersistence legacyCachePersistence, string clientId)
	{
		List<AdalUserForMsalEntry> userEntries = new List<AdalUserForMsalEntry>();
		try
		{
			(from p in AdalCacheOperations.Deserialize(logger, legacyCachePersistence.LoadCache())
				where p.Key.ClientId.Equals(clientId, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(p.Key.Authority)
				select p).ToList().ForEach(delegate(KeyValuePair<AdalTokenCacheKey, AdalResultWrapper> kvp)
			{
				userEntries.Add(new AdalUserForMsalEntry(authority: kvp.Key.Authority, clientId: clientId, clientInfo: kvp.Value.RawClientInfo, userInfo: kvp.Value.Result.UserInfo));
			});
		}
		catch (Exception exWithPii)
		{
			logger.WarningPiiWithPrefix(exWithPii, "An error occurred while reading accounts in ADAL format from the cache for MSAL. For details please see https://aka.ms/net-cache-persistence-errors. ");
		}
		return new AdalUsersForMsal(userEntries);
	}

	public static void RemoveAdalUser(ICoreLogger logger, ILegacyCachePersistence legacyCachePersistence, string clientId, string displayableId, string accountOrUserId)
	{
		try
		{
			IDictionary<AdalTokenCacheKey, AdalResultWrapper> dictionary = AdalCacheOperations.Deserialize(logger, legacyCachePersistence.LoadCache());
			if (!string.IsNullOrEmpty(accountOrUserId))
			{
				RemoveEntriesWithMatchingId(clientId, accountOrUserId, dictionary);
			}
			RemoveEntriesWithMatchingName(logger, clientId, displayableId, dictionary);
			legacyCachePersistence.WriteCache(AdalCacheOperations.Serialize(logger, dictionary));
		}
		catch (Exception exWithPii)
		{
			logger.WarningPiiWithPrefix(exWithPii, "An error occurred while deleting account in ADAL format from the cache. For details please see https://aka.ms/net-cache-persistence-errors. ");
		}
	}

	private static void RemoveEntriesWithMatchingName(ICoreLogger logger, string clientId, string displayableId, IDictionary<AdalTokenCacheKey, AdalResultWrapper> adalCache)
	{
		if (string.IsNullOrEmpty(displayableId))
		{
			logger.Error("Internal error - trying to remove an MSAL user with an empty username. Possible cache corruption. See https://aka.ms/adal_token_cache_serialization");
			return;
		}
		List<AdalTokenCacheKey> list = new List<AdalTokenCacheKey>();
		foreach (KeyValuePair<AdalTokenCacheKey, AdalResultWrapper> item in adalCache)
		{
			string displayableId2 = item.Key.DisplayableId;
			string clientId2 = item.Key.ClientId;
			if (string.Equals(displayableId, displayableId2, StringComparison.OrdinalIgnoreCase) && string.Equals(clientId, clientId2, StringComparison.OrdinalIgnoreCase))
			{
				list.Add(item.Key);
			}
		}
		foreach (AdalTokenCacheKey item2 in list)
		{
			adalCache.Remove(item2);
		}
	}

	private static void RemoveEntriesWithMatchingId(string clientId, string accountOrUserId, IDictionary<AdalTokenCacheKey, AdalResultWrapper> adalCache)
	{
		List<AdalTokenCacheKey> list = new List<AdalTokenCacheKey>();
		foreach (KeyValuePair<AdalTokenCacheKey, AdalResultWrapper> item in adalCache)
		{
			string rawClientInfo = item.Value.RawClientInfo;
			if (!string.IsNullOrEmpty(rawClientInfo))
			{
				string b = ClientInfo.CreateFromJson(rawClientInfo).ToAccountIdentifier();
				string clientId2 = item.Key.ClientId;
				if (string.Equals(accountOrUserId, b, StringComparison.OrdinalIgnoreCase) && string.Equals(clientId, clientId2, StringComparison.OrdinalIgnoreCase))
				{
					list.Add(item.Key);
				}
			}
		}
		foreach (AdalTokenCacheKey item2 in list)
		{
			adalCache.Remove(item2);
		}
	}

	public static MsalRefreshTokenCacheItem GetRefreshToken(ICoreLogger logger, ILegacyCachePersistence legacyCachePersistence, IEnumerable<string> environmentAliases, string clientId, IAccount account)
	{
		try
		{
			IEnumerable<KeyValuePair<AdalTokenCacheKey, AdalResultWrapper>> source = from p in AdalCacheOperations.Deserialize(logger, legacyCachePersistence.LoadCache())
				where p.Key.ClientId.Equals(clientId, StringComparison.OrdinalIgnoreCase) && environmentAliases.Contains(new Uri(p.Key.Authority).Host)
				select p;
			bool flag = false;
			if (!string.IsNullOrEmpty(account?.Username))
			{
				source = source.Where((KeyValuePair<AdalTokenCacheKey, AdalResultWrapper> p) => account.Username.Equals(p.Key.DisplayableId, StringComparison.OrdinalIgnoreCase));
				flag = true;
			}
			if (!string.IsNullOrEmpty(account?.HomeAccountId?.ObjectId))
			{
				source = source.Where((KeyValuePair<AdalTokenCacheKey, AdalResultWrapper> p) => account.HomeAccountId.ObjectId.Equals(p.Key.UniqueId, StringComparison.OrdinalIgnoreCase)).ToList();
				flag = true;
			}
			if (!flag)
			{
				logger.Warning("Could not filter ADAL entries by either UPN or unique ID, skipping.");
				return null;
			}
			return source.Select((KeyValuePair<AdalTokenCacheKey, AdalResultWrapper> adalEntry) => new MsalRefreshTokenCacheItem(new Uri(adalEntry.Key.Authority).Host, adalEntry.Key.ClientId, adalEntry.Value.RefreshToken, adalEntry.Value.RawClientInfo, null, GetHomeAccountId(adalEntry.Value))).FirstOrDefault();
		}
		catch (Exception exWithPii)
		{
			logger.WarningPiiWithPrefix(exWithPii, "An error occurred while searching for refresh tokens in ADAL format in the cache for MSAL. For details please see https://aka.ms/net-cache-persistence-errors. ");
			return null;
		}
	}

	private static string GetHomeAccountId(AdalResultWrapper adalResultWrapper)
	{
		if (!string.IsNullOrEmpty(adalResultWrapper.RawClientInfo))
		{
			return ClientInfo.CreateFromJson(adalResultWrapper.RawClientInfo).ToAccountIdentifier();
		}
		return null;
	}
}
