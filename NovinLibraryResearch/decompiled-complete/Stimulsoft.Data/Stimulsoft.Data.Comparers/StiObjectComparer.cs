using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Data.Types;

namespace Stimulsoft.Data.Comparers;

public class StiObjectComparer : IEqualityComparer<object>
{
	public static readonly StiObjectComparer Default = new StiObjectComparer();

	public new bool Equals(object x, object y)
	{
		return Compare(x, y) == 0;
	}

	public int GetHashCode(object x)
	{
		return ((IStructuralEquatable)x).GetHashCode(EqualityComparer<object>.Default);
	}

	public static int Compare(object x, object y)
	{
		if (x == DBNull.Value && y == DBNull.Value)
		{
			return 0;
		}
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == DBNull.Value)
		{
			return -1;
		}
		if (y == DBNull.Value)
		{
			return 1;
		}
		if (x == null)
		{
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		Type type = x.GetType();
		if (type == typeof(DateTime))
		{
			return DateTimeCompare(x, y);
		}
		if (type == typeof(string))
		{
			return string.Compare(x.ToString(), y.ToString(), CultureInfo.CurrentCulture, CompareOptions.OrdinalIgnoreCase);
		}
		if (type == typeof(bool))
		{
			return DefaultCompare<bool>(x, y);
		}
		if (type == typeof(short) || type == typeof(int) || type == typeof(long) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
		{
			return Comparer<long>.Default.Compare(StiValueHelper.TryToLong(x), StiValueHelper.TryToLong(y));
		}
		if (type == typeof(float) || type == typeof(double))
		{
			return Comparer<double>.Default.Compare(StiValueHelper.TryToDouble(x), StiValueHelper.TryToDouble(y));
		}
		if (type == typeof(decimal))
		{
			return Comparer<decimal>.Default.Compare(StiValueHelper.TryToDecimal(x), StiValueHelper.TryToDecimal(y));
		}
		if (type == typeof(byte[]))
		{
			return ArrayCompare(x as byte[], y as byte[]);
		}
		if (x is SimpleValue || y is SimpleValue)
		{
			object x2 = ((x is SimpleValue) ? ((SimpleValue)x).Value : x);
			object y2 = ((y is SimpleValue) ? ((SimpleValue)y).Value : y);
			return Compare(x2, y2);
		}
		if (x is DateTimeValue || y is DateTimeValue)
		{
			object x3 = ((x is DateTimeValue) ? ((object)((DateTimeValue)x).Value) : x);
			object y3 = ((y is DateTimeValue) ? ((object)((DateTimeValue)y).Value) : y);
			return Compare(x3, y3);
		}
		return Comparer.Default.Compare(x, y);
	}

	private static int DefaultCompare<T>(object a, object b)
	{
		return Comparer<T>.Default.Compare((T)a, (T)b);
	}

	private static int DateTimeCompare(object a, object b)
	{
		if (a is DateTime && b is DateTime)
		{
			return Comparer<DateTime>.Default.Compare(StiDateTimeCorrector.Correct((DateTime)a), StiDateTimeCorrector.Correct((DateTime)b));
		}
		if (a is DateTime && !(b is DateTime))
		{
			return -1;
		}
		return 1;
	}

	private static int ArrayCompare(byte[] a, byte[] b)
	{
		if (a.Length < b.Length)
		{
			return -1;
		}
		if (a.Length > b.Length)
		{
			return 1;
		}
		if (!a.AsQueryable().SequenceEqual(b))
		{
			return -1;
		}
		return 0;
	}
}
