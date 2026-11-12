using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.Identity.Client;

public sealed class TokenCacheNotificationArgs
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[Obsolete("Use Account instead (See https://aka.ms/msal-net-2-released)", true)]
	public IUser User
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ITokenCacheSerializer TokenCache { get; }

	public string ClientId { get; }

	public IAccount Account { get; }

	public bool HasStateChanged { get; internal set; }

	public bool IsApplicationCache { get; }

	public string SuggestedCacheKey { get; }

	public bool HasTokens { get; }

	internal TokenCacheNotificationArgs(ITokenCacheSerializer tokenCacheSerializer, string clientId, IAccount account, bool hasStateChanged, bool isAppCache, bool hasTokens, string suggestedCacheKey = null)
	{
		TokenCache = tokenCacheSerializer;
		ClientId = clientId;
		Account = account;
		HasStateChanged = hasStateChanged;
		IsApplicationCache = isAppCache;
		HasTokens = hasTokens;
		SuggestedCacheKey = suggestedCacheKey;
	}
}
