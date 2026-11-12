using System;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory;

public static class CacheEntryExtensions
{
	public static ICacheEntry SetPriority(this ICacheEntry entry, CacheItemPriority priority)
	{
		entry.Priority = priority;
		return entry;
	}

	public static ICacheEntry AddExpirationToken(this ICacheEntry entry, IChangeToken expirationToken)
	{
		if (expirationToken == null)
		{
			throw new ArgumentNullException("expirationToken");
		}
		entry.ExpirationTokens.Add(expirationToken);
		return entry;
	}

	public static ICacheEntry SetAbsoluteExpiration(this ICacheEntry entry, TimeSpan relative)
	{
		entry.AbsoluteExpirationRelativeToNow = relative;
		return entry;
	}

	public static ICacheEntry SetAbsoluteExpiration(this ICacheEntry entry, DateTimeOffset absolute)
	{
		entry.AbsoluteExpiration = absolute;
		return entry;
	}

	public static ICacheEntry SetSlidingExpiration(this ICacheEntry entry, TimeSpan offset)
	{
		entry.SlidingExpiration = offset;
		return entry;
	}

	public static ICacheEntry RegisterPostEvictionCallback(this ICacheEntry entry, PostEvictionDelegate callback)
	{
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		return entry.RegisterPostEvictionCallback(callback, null);
	}

	public static ICacheEntry RegisterPostEvictionCallback(this ICacheEntry entry, PostEvictionDelegate callback, object state)
	{
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		entry.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration
		{
			EvictionCallback = callback,
			State = state
		});
		return entry;
	}

	public static ICacheEntry SetValue(this ICacheEntry entry, object value)
	{
		entry.Value = value;
		return entry;
	}

	public static ICacheEntry SetSize(this ICacheEntry entry, long size)
	{
		if (size < 0)
		{
			throw new ArgumentOutOfRangeException("size", size, "size must be non-negative.");
		}
		entry.Size = size;
		return entry;
	}

	public static ICacheEntry SetOptions(this ICacheEntry entry, MemoryCacheEntryOptions options)
	{
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		entry.AbsoluteExpiration = options.AbsoluteExpiration;
		entry.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
		entry.SlidingExpiration = options.SlidingExpiration;
		entry.Priority = options.Priority;
		entry.Size = options.Size;
		foreach (IChangeToken expirationToken in options.ExpirationTokens)
		{
			entry.AddExpirationToken(expirationToken);
		}
		foreach (PostEvictionCallbackRegistration postEvictionCallback in options.PostEvictionCallbacks)
		{
			entry.RegisterPostEvictionCallback(postEvictionCallback.EvictionCallback, postEvictionCallback.State);
		}
		return entry;
	}
}
