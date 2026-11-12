using System;
using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class PropertyListComparer : IComparer<IReadOnlyList<IProperty>>, IEqualityComparer<IReadOnlyList<IProperty>>
{
	public static readonly PropertyListComparer Instance = new PropertyListComparer();

	private PropertyListComparer()
	{
	}

	public int Compare(IReadOnlyList<IProperty> x, IReadOnlyList<IProperty> y)
	{
		int num = x.Count - y.Count;
		if (num != 0)
		{
			return num;
		}
		int num2 = 0;
		while (num == 0 && num2 < x.Count)
		{
			num = StringComparer.Ordinal.Compare(x[num2].Name, y[num2].Name);
			num2++;
		}
		return num;
	}

	public bool Equals(IReadOnlyList<IProperty> x, IReadOnlyList<IProperty> y)
	{
		return Compare(x, y) == 0;
	}

	public int GetHashCode(IReadOnlyList<IProperty> obj)
	{
		HashCode hashCode = default(HashCode);
		for (int i = 0; i < obj.Count; i++)
		{
			hashCode.Add(obj[i]);
		}
		return hashCode.ToHashCode();
	}
}
