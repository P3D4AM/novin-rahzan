using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Internal;

public sealed class ReferenceEnumerableEqualityComparer<TEnumerable, TValue> : IEqualityComparer<TEnumerable> where TEnumerable : IEnumerable<TValue>
{
	public bool Equals(TEnumerable x, TEnumerable y)
	{
		return x.SequenceEqual(y);
	}

	public int GetHashCode(TEnumerable obj)
	{
		HashCode hashCode = default(HashCode);
		foreach (TValue item in obj)
		{
			hashCode.Add(item);
		}
		return hashCode.ToHashCode();
	}
}
