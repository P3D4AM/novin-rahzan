using System;
using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class EntityTypePathComparer : IComparer<IEntityType>, IEqualityComparer<IEntityType>
{
	public static readonly EntityTypePathComparer Instance = new EntityTypePathComparer();

	private EntityTypePathComparer()
	{
	}

	public virtual int Compare(IEntityType x, IEntityType y)
	{
		if (x == y)
		{
			return 0;
		}
		if (x == null)
		{
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		int num = StringComparer.Ordinal.Compare(x.Name, y.Name);
		if (num != 0)
		{
			return num;
		}
		while (true)
		{
			string definingNavigationName = x.DefiningNavigationName;
			string definingNavigationName2 = y.DefiningNavigationName;
			if (definingNavigationName == null && definingNavigationName2 == null)
			{
				return StringComparer.Ordinal.Compare(x.Name, y.Name);
			}
			if (definingNavigationName == null)
			{
				return -1;
			}
			if (definingNavigationName2 == null)
			{
				return 1;
			}
			num = StringComparer.Ordinal.Compare(definingNavigationName, definingNavigationName2);
			if (num != 0)
			{
				break;
			}
			x = x.DefiningEntityType;
			y = y.DefiningEntityType;
		}
		return num;
	}

	public bool Equals(IEntityType x, IEntityType y)
	{
		return Compare(x, y) == 0;
	}

	public virtual int GetHashCode(IEntityType entityType)
	{
		HashCode hashCode = default(HashCode);
		while (true)
		{
			hashCode.Add<string>(entityType.Name, StringComparer.Ordinal);
			string definingNavigationName = entityType.DefiningNavigationName;
			if (definingNavigationName == null)
			{
				break;
			}
			hashCode.Add<string>(definingNavigationName, StringComparer.Ordinal);
			entityType = entityType.DefiningEntityType;
		}
		return hashCode.ToHashCode();
	}
}
