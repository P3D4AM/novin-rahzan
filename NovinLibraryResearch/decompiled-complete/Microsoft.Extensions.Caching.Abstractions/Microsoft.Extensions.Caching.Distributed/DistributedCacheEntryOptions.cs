using System;

namespace Microsoft.Extensions.Caching.Distributed;

public class DistributedCacheEntryOptions
{
	private DateTimeOffset? _absoluteExpiration;

	private TimeSpan? _absoluteExpirationRelativeToNow;

	private TimeSpan? _slidingExpiration;

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
}
