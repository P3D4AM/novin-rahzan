using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Cache;

namespace Microsoft.Identity.Client;

public interface ITokenCache
{
	void SetBeforeAccess(TokenCacheCallback beforeAccess);

	void SetAfterAccess(TokenCacheCallback afterAccess);

	void SetBeforeWrite(TokenCacheCallback beforeWrite);

	void SetBeforeAccessAsync(Func<TokenCacheNotificationArgs, Task> beforeAccess);

	void SetAfterAccessAsync(Func<TokenCacheNotificationArgs, Task> afterAccess);

	void SetBeforeWriteAsync(Func<TokenCacheNotificationArgs, Task> beforeWrite);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use ITokenCacheSerializer.SerializeMsalV3 on the TokenCacheNotificationArgs in the cache callback. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	byte[] SerializeMsalV3();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use ITokenCacheSerializer.DeserializeMsalV3 on the TokenCacheNotificationArgs in the cache callback. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	void DeserializeMsalV3(byte[] msalV3State, bool shouldClearExistingCache = false);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use ITokenCacheSerializer.SerializeMsalV2 on the TokenCacheNotificationArgs in the cache callback. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	byte[] SerializeMsalV2();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use ITokenCacheSerializer.DeserializeMsalV2 on the TokenCacheNotificationArgs in the cache callback. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	void DeserializeMsalV2(byte[] msalV2State);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use ITokenCacheSerializer.SerializeAdalV3 on the TokenCacheNotificationArgs in the cache callback. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	byte[] SerializeAdalV3();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use ITokenCacheSerializer.DeserializeAdalV3 on the TokenCacheNotificationArgs in the cache callback. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	void DeserializeAdalV3(byte[] adalV3State);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is expected to be removed in MSAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	byte[] Serialize();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is expected to be removed in MSAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	void Deserialize(byte[] msalV2State);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is expected to be removed in MSAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	CacheData SerializeUnifiedAndAdalCache();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This is expected to be removed in MSAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-4x-cache-breaking-change", false)]
	void DeserializeUnifiedAndAdalCache(CacheData cacheData);
}
