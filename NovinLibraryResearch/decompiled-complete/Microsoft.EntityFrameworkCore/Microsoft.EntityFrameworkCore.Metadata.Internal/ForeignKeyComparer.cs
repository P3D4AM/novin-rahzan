using System;
using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ForeignKeyComparer : IEqualityComparer<IForeignKey>, IComparer<IForeignKey>
{
	public static readonly ForeignKeyComparer Instance = new ForeignKeyComparer();

	private ForeignKeyComparer()
	{
	}

	public virtual int Compare(IForeignKey x, IForeignKey y)
	{
		int num = PropertyListComparer.Instance.Compare(x.Properties, y.Properties);
		if (num != 0)
		{
			return num;
		}
		num = PropertyListComparer.Instance.Compare(x.PrincipalKey.Properties, y.PrincipalKey.Properties);
		if (num != 0)
		{
			return num;
		}
		num = EntityTypePathComparer.Instance.Compare(x.PrincipalEntityType, y.PrincipalEntityType);
		if (num == 0)
		{
			return EntityTypePathComparer.Instance.Compare(x.DeclaringEntityType, y.DeclaringEntityType);
		}
		return num;
	}

	public virtual bool Equals(IForeignKey x, IForeignKey y)
	{
		return Compare(x, y) == 0;
	}

	public virtual int GetHashCode(IForeignKey obj)
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add(obj.PrincipalKey.Properties, PropertyListComparer.Instance);
		hashCode.Add(obj.Properties, PropertyListComparer.Instance);
		hashCode.Add(obj.PrincipalEntityType, EntityTypePathComparer.Instance);
		hashCode.Add(obj.DeclaringEntityType, EntityTypePathComparer.Instance);
		return hashCode.ToHashCode();
	}
}
