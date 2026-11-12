using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class ReferenceComparer<T> : IEqualityComparer<T>
{
	public static ReferenceComparer<T> Instance { get; } = new ReferenceComparer<T>();


	private ReferenceComparer()
	{
	}

	public bool Equals(T x, T y)
	{
		return (object)x == (object)y;
	}

	public int GetHashCode(T obj)
	{
		return obj.GetHashCode();
	}
}
