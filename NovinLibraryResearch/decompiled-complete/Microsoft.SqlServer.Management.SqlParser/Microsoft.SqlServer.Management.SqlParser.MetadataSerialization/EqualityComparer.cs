using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class EqualityComparer : IEqualityComparer<CollationInfo>, IEqualityComparer<ComputedColumnInfo>, IEqualityComparer<DataTypeSpec>, IEqualityComparer<IdentityColumnInfo>, IEqualityComparer<IPassword>
{
	private static class SingletonHelper
	{
		public static readonly EqualityComparer Instance;

		static SingletonHelper()
		{
			Instance = new EqualityComparer();
		}
	}

	public static IEqualityComparer<byte[]> ByteArray => EqualityComparer<byte[]>.Default;

	public static IEqualityComparer<CollationInfo> CollationInfo => SingletonHelper.Instance;

	public static IEqualityComparer<ComputedColumnInfo> ComputedColumnInfo => SingletonHelper.Instance;

	public static IEqualityComparer<DataTypeSpec> DataTypeSpec => SingletonHelper.Instance;

	public static IEqualityComparer<double> Double => EqualityComparer<double>.Default;

	public static IEqualityComparer<GridDensity> GridDensity => EqualityComparer<Microsoft.SqlServer.Management.SqlParser.Metadata.GridDensity>.Default;

	public static IEqualityComparer<IdentityColumnInfo> IdentityColumnInfo => SingletonHelper.Instance;

	public static IEqualityComparer<IndexType> IndexType => EqualityComparer<Microsoft.SqlServer.Management.SqlParser.Metadata.IndexType>.Default;

	public static IEqualityComparer<int> Int => EqualityComparer<int>.Default;

	public static IEqualityComparer<int[]> IntArray => EqualityComparer<int[]>.Default;

	public static IEqualityComparer<IPassword> Password => SingletonHelper.Instance;

	public static IEqualityComparer<string> String => StringComparer.Ordinal;

	public static IEqualityComparer<StatisticsType> StatisticsType => EqualityComparer<Microsoft.SqlServer.Management.SqlParser.Metadata.StatisticsType>.Default;

	private EqualityComparer()
	{
	}

	bool IEqualityComparer<CollationInfo>.Equals(CollationInfo x, CollationInfo y)
	{
		if (x == null)
		{
			return y == null;
		}
		if (y == null)
		{
			return false;
		}
		return StringComparer.Ordinal.Equals(x.Name, y.Name);
	}

	int IEqualityComparer<CollationInfo>.GetHashCode(CollationInfo obj)
	{
		return obj?.Name.GetHashCode() ?? 0;
	}

	bool IEqualityComparer<ComputedColumnInfo>.Equals(ComputedColumnInfo x, ComputedColumnInfo y)
	{
		if (x == null)
		{
			return y == null;
		}
		if (y == null)
		{
			return false;
		}
		if (x.IsPersisted == y.IsPersisted)
		{
			return StringComparer.Ordinal.Equals(x.Text, y.Text);
		}
		return false;
	}

	int IEqualityComparer<ComputedColumnInfo>.GetHashCode(ComputedColumnInfo obj)
	{
		if (obj == null)
		{
			return 0;
		}
		return obj.IsPersisted.GetHashCode() ^ obj.Text.GetHashCode();
	}

	bool IEqualityComparer<DataTypeSpec>.Equals(DataTypeSpec x, DataTypeSpec y)
	{
		if (x == null)
		{
			return y == null;
		}
		if (y == null)
		{
			return false;
		}
		return StringComparer.Ordinal.Equals(x.Name, y.Name);
	}

	int IEqualityComparer<DataTypeSpec>.GetHashCode(DataTypeSpec obj)
	{
		return obj?.Name.GetHashCode() ?? 0;
	}

	public bool Equals(IdentityColumnInfo x, IdentityColumnInfo y)
	{
		if (x == null)
		{
			return y == null;
		}
		if (y == null)
		{
			return false;
		}
		if (x.Increment == y.Increment && x.Seed == y.Seed)
		{
			return x.NotForReplication == y.NotForReplication;
		}
		return false;
	}

	public int GetHashCode(IdentityColumnInfo obj)
	{
		if (obj == null)
		{
			return 0;
		}
		return obj.Increment.GetHashCode() ^ obj.Seed.GetHashCode();
	}

	public bool Equals(IPassword x, IPassword y)
	{
		if (x == null)
		{
			return y == null;
		}
		if (y == null)
		{
			return false;
		}
		if (x.CheckExpiration == y.CheckExpiration && x.CheckPolicy == y.CheckPolicy && x.IsHashed == y.IsHashed && x.MustChange == y.MustChange)
		{
			return x.Value == y.Value;
		}
		return false;
	}

	public int GetHashCode(IPassword obj)
	{
		int num;
		if (obj != null)
		{
			num = (obj.CheckExpiration ? 1 : 0);
			num = (obj.CheckPolicy ? 1 : 0) << 1;
			num = (obj.IsHashed ? 1 : 0) << 2;
			num = (obj.MustChange ? 1 : 0) << 3;
			if (obj.Value != null)
			{
				num ^= obj.Value.GetHashCode();
			}
		}
		else
		{
			num = 0;
		}
		return num;
	}
}
