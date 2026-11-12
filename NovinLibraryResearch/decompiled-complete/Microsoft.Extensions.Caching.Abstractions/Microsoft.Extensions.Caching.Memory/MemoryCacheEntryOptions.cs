using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.Caching.Memory;

public class MemoryCacheEntryOptions
{
	private DateTimeOffset? _absoluteExpiration;

	private TimeSpan? _absoluteExpirationRelativeToNow;

	private TimeSpan? _slidingExpiration;

	private long? _size;

	public DateTimeOffset? AbsoluteExpiration
	{
		get
		{
			return _absoluteExpiration;
		}
		set
		{
			_absoluteExpiration = value;
		}
	}

	public TimeSpan? AbsoluteExpirationRelativeToNow
	{
		get
		{
			return _absoluteExpirationRelativeToNow;
		}
		set
		{
			if (value <= TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("AbsoluteExpirationRelativeToNow", value, "The relative expiration value must be positive.");
			}
			_absoluteExpirationRelativeToNow = value;
		}
	}

	public TimeSpan? SlidingExpiration
	{
		get
		{
			return _slidingExpiration;
		}
		set
		{
			if (value <= TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("SlidingExpiration", value, "The sliding expiration value must be positive.");
			}
			_slidingExpiration = value;
		}
	}

	public IList<IChangeToken> ExpirationTokens { get; } = new List<IChangeToken>();


	public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<PostEvictionCallbackRegistration>();


	public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;


	public long? Size
	{
		get
		{
			return _size;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value", value, "value must be non-negative.");
			}
			_size = value;
		}
	}
}
