using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.EntityFrameworkCore.Internal;

public sealed class ReferenceEqualityComparer : IEqualityComparer<object>
{
	public static ReferenceEqualityComparer Instance { get; } = new ReferenceEqualityComparer();


	private ReferenceEqualityComparer()
	{
	}

	bool IEqualityComparer<object>.Equals(object x, object y)
	{
		return x == y;
	}

	int IEqualityComparer<object>.GetHashCode(object obj)
	{
		return RuntimeHelpers.GetHashCode(obj);
	}
}
