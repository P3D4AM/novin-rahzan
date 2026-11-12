using System;
using System.Reflection;

namespace Microsoft.Data.SqlClient.Server;

internal sealed class FieldInfoEx : IComparable
{
	internal readonly int offset;

	internal readonly FieldInfo fieldInfo;

	internal readonly Normalizer normalizer;

	internal FieldInfoEx(FieldInfo fi, int offset, Normalizer normalizer)
	{
		fieldInfo = fi;
		this.offset = offset;
		this.normalizer = normalizer;
	}

	public int CompareTo(object other)
	{
		if (!(other is FieldInfoEx fieldInfoEx))
		{
			return -1;
		}
		int num = offset;
		return num.CompareTo(fieldInfoEx.offset);
	}
}
