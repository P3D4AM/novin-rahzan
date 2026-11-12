using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Client.Platforms.net45.Http;

internal struct EndpointCacheKey : IEquatable<EndpointCacheKey>
{
	private readonly Uri _uri;

	public EndpointCacheKey(Uri uri)
	{
		_uri = uri;
	}

	public bool Equals(EndpointCacheKey other)
	{
		if (string.Equals(_uri.Host, other._uri.Host, StringComparison.OrdinalIgnoreCase) && string.Equals(_uri.Scheme, other._uri.Scheme, StringComparison.OrdinalIgnoreCase))
		{
			return _uri.Port == other._uri.Port;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is EndpointCacheKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((-1793347351 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_uri.Scheme)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_uri.Host)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(_uri.Port);
	}

	public static bool operator ==(EndpointCacheKey left, EndpointCacheKey right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(EndpointCacheKey left, EndpointCacheKey right)
	{
		return !left.Equals(right);
	}
}
