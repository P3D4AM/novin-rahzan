using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Serilog.Core.Pipeline;

internal class ByReferenceStringComparer : IEqualityComparer, IEqualityComparer<string>
{
	public static readonly ByReferenceStringComparer Instance = new ByReferenceStringComparer();

	private ByReferenceStringComparer()
	{
	}

	public new bool Equals(object? x, object? y)
	{
		return x == y;
	}

	public int GetHashCode(object obj)
	{
		return RuntimeHelpers.GetHashCode(obj);
	}

	public bool Equals(string? x, string? y)
	{
		return (object)x == y;
	}

	public int GetHashCode(string obj)
	{
		return GetHashCode((object)obj);
	}
}
