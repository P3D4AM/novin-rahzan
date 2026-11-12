using System;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory;

public static class MemoryCacheEntryExtensions
{
	public static MemoryCacheEntryOptions SetPriority(this MemoryCacheEntryOptions options, CacheItemPriority priority)
	{
		options.Priority = priority;
		return options;
	}

	public static MemoryCacheEntryOptions SetSize(this MemoryCacheEntryOptions options, long size)
	{
		if (size < 0)
		{
			throw new ArgumentOutOfRangeException("size", size, "size must be non-negative.");
		}
		options.Size = size;
		return options;
	}

	public static MemoryCacheEntryOptions AddExpirationToken(this MemoryCacheEntryOptions options, IChangeToken expirationToken)
	{
		if (expirationToken == null)
		{
			throw new ArgumentNullException("expirationToken");
		}
		options.ExpirationTokens.Add(expirationToken);
		return options;
	}

	public static MemoryCacheEntryOptions SetAbsoluteExpiration(this MemoryCacheEntryOptions options, TimeSpan relative)
	{
		options.AbsoluteExpirationRelativeToNow = relative;
		return options;
	}

	public static MemoryCacheEntryOptions SetAbsoluteExpiration(this MemoryCacheEntryOptions options, DateTimeOffset absolute)
	{
		options.AbsoluteExpiration = absolute;
		return options;
	}

	public static MemoryCacheEntryOptions SetSlidingExpiration(this MemoryCacheEntryOptions options, TimeSpan offset)
	{
		options.SlidingExpiration = offset;
		return options;
	}

	public static MemoryCacheEntryOptions RegisterPostEvictionCallback(this MemoryCacheEntryOptions options, PostEvictionDelegate callback)
	{
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		return options.RegisterPostEvictionCallback(callback, null);
	}

	public static MemoryCacheEntryOptions RegisterPostEvictionCallback(this MemoryCacheEntryOptions options, PostEvictionDelegate callback, object state)
	{
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		options.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration
		{
			EvictionCallback = callback,
			State = state
		});
		return options;
	}
}
