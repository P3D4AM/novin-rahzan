using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup;

internal readonly struct ServiceCacheKey : IEquatable<ServiceCacheKey>
{
	public static ServiceCacheKey Empty { get; } = new ServiceCacheKey(null, 0);


	public Type? Type { get; }

	public int Slot { get; }

	public ServiceCacheKey(Type? type, int slot)
	{
		Type = type;
		Slot = slot;
	}

	public bool Equals(ServiceCacheKey other)
	{
		if (Type == other.Type)
		{
			return Slot == other.Slot;
		}
		return false;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		if (obj is ServiceCacheKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((Type?.GetHashCode() ?? 23) * 397) ^ Slot;
	}
}
