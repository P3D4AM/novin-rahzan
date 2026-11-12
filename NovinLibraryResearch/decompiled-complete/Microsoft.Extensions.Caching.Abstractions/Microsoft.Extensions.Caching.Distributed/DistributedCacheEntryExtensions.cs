using System;

namespace Microsoft.Extensions.Caching.Distributed;

public static class DistributedCacheEntryExtensions
{
	public static DistributedCacheEntryOptions SetAbsoluteExpiration(this DistributedCacheEntryOptions options, TimeSpan relative)
	{
		options.AbsoluteExpirationRelativeToNow = relative;
		return options;
	}

	public static DistributedCacheEntryOptions SetAbsoluteExpiration(this DistributedCacheEntryOptions options, DateTimeOffset absolute)
	{
		options.AbsoluteExpiration = absolute;
		return options;
	}

	public static DistributedCacheEntryOptions SetSlidingExpiration(this DistributedCacheEntryOptions options, TimeSpan offset)
	{
		options.SlidingExpiration = offset;
		return options;
	}
}
