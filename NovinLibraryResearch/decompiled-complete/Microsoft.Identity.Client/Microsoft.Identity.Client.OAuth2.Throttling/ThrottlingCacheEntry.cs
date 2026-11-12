using System;

namespace Microsoft.Identity.Client.OAuth2.Throttling;

internal class ThrottlingCacheEntry
{
	public MsalServiceException Exception { get; }

	public DateTimeOffset CreationTime { get; }

	public DateTimeOffset ExpirationTime { get; }

	public bool IsExpired
	{
		get
		{
			if (!(ExpirationTime < DateTimeOffset.Now))
			{
				return CreationTime > DateTimeOffset.Now;
			}
			return true;
		}
	}

	public ThrottlingCacheEntry(MsalServiceException exception, TimeSpan lifetime)
	{
		Exception = exception ?? throw new ArgumentNullException("exception");
		CreationTime = DateTimeOffset.UtcNow;
		ExpirationTime = CreationTime.Add(lifetime);
	}

	public ThrottlingCacheEntry(MsalServiceException exception, DateTimeOffset creationTime, DateTimeOffset expirationTime)
	{
		Exception = exception ?? throw new ArgumentNullException("exception");
		CreationTime = creationTime;
		ExpirationTime = expirationTime;
	}
}
