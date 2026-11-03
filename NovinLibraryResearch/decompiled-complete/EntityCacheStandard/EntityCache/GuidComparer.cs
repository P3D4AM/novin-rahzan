using System.Collections.Generic;
using PacketParser.Entities.Interfaces;

namespace EntityCache;

internal class GuidComparer<T> : IEqualityComparer<T> where T : IHasGuid
{
	public bool Equals(T x, T y)
	{
		return x.Guid == y.Guid;
	}

	public int GetHashCode(T obj)
	{
		return obj.Guid.GetHashCode();
	}
}
