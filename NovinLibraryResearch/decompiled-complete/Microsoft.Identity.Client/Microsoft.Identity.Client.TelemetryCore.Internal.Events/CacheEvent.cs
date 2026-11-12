using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal.Events;

internal class CacheEvent : EventBase
{
	public enum TokenTypes
	{
		AT,
		RT,
		ID,
		Account,
		AppMetadata
	}

	public const string TokenCacheLookup = "msal.token_cache_lookup";

	public const string TokenCacheWrite = "msal.token_cache_write";

	public const string TokenCacheBeforeAccess = "msal.token_cache_before_access";

	public const string TokenCacheAfterAccess = "msal.token_cache_after_access";

	public const string TokenCacheBeforeWrite = "msal.token_cache_before_write";

	public const string TokenCacheDelete = "msal.token_cache_delete";

	public const string TokenTypeKey = "msal.token_type";

	public TokenTypes TokenType
	{
		set
		{
			Dictionary<TokenTypes, string> dictionary = new Dictionary<TokenTypes, string>
			{
				{
					TokenTypes.AT,
					"at"
				},
				{
					TokenTypes.RT,
					"rt"
				},
				{
					TokenTypes.ID,
					"id"
				},
				{
					TokenTypes.Account,
					"account"
				},
				{
					TokenTypes.AppMetadata,
					"appmetadata"
				}
			};
			base["msal.token_type"] = dictionary[value];
		}
	}

	public CacheEvent(string eventName, string correlationId)
		: base(eventName, correlationId)
	{
	}
}
