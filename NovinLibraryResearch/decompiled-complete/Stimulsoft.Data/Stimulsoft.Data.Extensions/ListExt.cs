using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Data.Extensions;

public static class ListExt
{
	public static object Compare(object a, object b)
	{
		if (!IsList(a) && !IsList(b))
		{
			return -1;
		}
		if (IsList(a) && !IsList(b))
		{
			return from aa in ToList(a)
				select CompareValues(aa, b);
		}
		if (!IsList(a) && IsList(b))
		{
			return from bb in ToList(b)
				select CompareValues(a, bb);
		}
		return (!ToList(a).SequenceEqual(ToList(b))) ? 1 : 0;
	}

	private static object CompareValues(object a, object b)
	{
		if (a is bool && b is bool)
		{
			return (bool)a == (bool)b;
		}
		if (a is string)
		{
			return b != null && (string)a == (string)b;
		}
		return Comparer.Default.Compare(a, b);
	}

	public static bool IsList(object value)
	{
		if (!(value is string))
		{
			return value is IEnumerable;
		}
		return false;
	}

	public static bool IsBoolList(object value)
	{
		return ToList(value)?.FirstOrDefault() is bool;
	}

	public static IEnumerable<object> ToList(object value)
	{
		return (value as IEnumerable)?.Cast<object>();
	}

	public static IEnumerable<string> ToStringList(object value)
	{
		return ToList(value)?.TryCastToString();
	}

	public static IEnumerable<int> ToIntList(object value)
	{
		return ToList(value)?.TryCastToInt();
	}

	public static IEnumerable<double> ToDoubleList(object value)
	{
		return ToList(value)?.TryCastToDouble();
	}

	public static IEnumerable<decimal> ToDecimalList(object value)
	{
		return ToList(value)?.TryCastToDecimal();
	}

	public static IEnumerable<DateTime?> ToNullableDateTimeList(object value)
	{
		return ToList(value)?.TryCastToNullableDateTime();
	}

	public static object[] ToArray(object value)
	{
		return ToList(value)?.ToArray();
	}

	public static string[] ToStringArray(object value)
	{
		return ToStringList(value)?.ToArray();
	}

	public static int[] ToIntArray(object value)
	{
		return ToIntList(value)?.ToArray();
	}

	public static double[] ToDoubleArray(object value)
	{
		return ToDoubleList(value)?.ToArray();
	}

	public static decimal[] ToDecimalArray(object value)
	{
		return ToDecimalList(value)?.ToArray();
	}

	public static object Add(object a, object b)
	{
		object[] array = ToArray(a);
		if (array == null)
		{
			return a;
		}
		for (int i = 0; i < array.Length; i++)
		{
			object obj = array[i];
			if (obj is byte)
			{
				array[i] = (byte)obj + StiValueHelper.TryToLong(b);
			}
			else if (obj is sbyte)
			{
				array[i] = (sbyte)obj + StiValueHelper.TryToLong(b);
			}
			else if (obj is short)
			{
				array[i] = (short)obj + StiValueHelper.TryToLong(b);
			}
			else if (obj is ushort)
			{
				array[i] = (ushort)obj + StiValueHelper.TryToLong(b);
			}
			else if (obj is int)
			{
				array[i] = (int)obj + StiValueHelper.TryToLong(b);
			}
			else if (obj is uint)
			{
				array[i] = (uint)obj + StiValueHelper.TryToLong(b);
			}
			else if (obj is long)
			{
				array[i] = (long)obj + StiValueHelper.TryToLong(b);
			}
			else if (obj is ulong)
			{
				array[i] = (ulong)obj + StiValueHelper.TryToULong(b);
			}
			else if (obj is float)
			{
				array[i] = (float)obj + StiValueHelper.TryToFloat(b);
			}
			else if (obj is double)
			{
				array[i] = (double)obj + StiValueHelper.TryToDouble(b);
			}
			else if (obj is decimal)
			{
				array[i] = (decimal)obj + StiValueHelper.TryToDecimal(b);
			}
		}
		return array.ToList();
	}
}
