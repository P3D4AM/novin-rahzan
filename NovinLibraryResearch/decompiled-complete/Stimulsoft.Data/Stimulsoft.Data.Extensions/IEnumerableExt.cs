using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Data.Extensions;

public static class IEnumerableExt
{
	public static IEnumerable<object[]> WhereEqualsTo(this IEnumerable<object[]> source, object values1, object values2)
	{
		return source.Where((object[] r) => StiValueComparer.EqualValues(values1, values2));
	}

	public static IEnumerable<object[]> WhereArrayItemEqualsTo(this IEnumerable<object[]> source, int itemIndex, object value)
	{
		if (itemIndex != -1)
		{
			return source.Where((object[] r) => StiValueComparer.EqualValues(r[itemIndex], value));
		}
		return source;
	}

	public static IEnumerable<object[]> WhereArrayItemStringEqualsTo(this IEnumerable<object[]> source, int itemIndex, string value)
	{
		if (itemIndex != -1)
		{
			return source.Where((object[] r) => StiValueComparer.EqualValues(ToString(r[itemIndex]), value));
		}
		return source;
	}

	public static object[] WhereFirstOrDefaultArrayItemStringEqualsTo(this IEnumerable<object[]> source, int itemIndex, string value)
	{
		if (itemIndex != -1)
		{
			return source.FirstOrDefault((object[] r) => StiValueComparer.EqualValues(ToString(r[itemIndex]), value));
		}
		return source.FirstOrDefault();
	}

	private static string ToString(object value)
	{
		if (value != null)
		{
			return value.ToString();
		}
		return string.Empty;
	}

	public static IEnumerable<object> GetArrayItem(this IEnumerable<object[]> source, int itemIndex)
	{
		return source.Select((object[] r) => r[itemIndex]);
	}

	public static IEnumerable<string> GetStringArrayItem(this IEnumerable<object[]> source, int itemIndex)
	{
		return source.Select((object[] r) => r[itemIndex] as string);
	}

	public static IEnumerable<double?> TryCastValueOrFirstDefaultToNullableDouble<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToNullableDouble(GetValueOrFirstOrDefault(c)));
	}

	public static IEnumerable<decimal?> TryCastToNullableDecimal<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToNullableDecimal(c));
	}

	public static IEnumerable<double?> TryCastToNullableDouble<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToNullableDouble(c));
	}

	public static IEnumerable<decimal> TryCastToDecimal<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToDecimal(c));
	}

	public static IEnumerable<double> TryCastToDouble<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToDouble(c));
	}

	public static IEnumerable<bool> TryCastToBool<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToBool(c));
	}

	public static IEnumerable<int> TryCastToInt<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToInt(c));
	}

	public static IEnumerable<long> TryCastToLong<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToLong(c));
	}

	public static IEnumerable<DateTime> TryCastToDateTime<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToDateTime(c));
	}

	public static IEnumerable<DateTime?> TryCastToNullableDateTime<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToNullableDateTime(c));
	}

	public static IEnumerable<TimeSpan?> TryCastToNullableTimeSpan<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToNullableTimeSpan(c));
	}

	public static IEnumerable<TimeSpan> TryCastToTimeSpan<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToTimeSpan(c));
	}

	public static IEnumerable<string> TryCastToString<TSource>(this IEnumerable<TSource> source)
	{
		return source.Select((TSource c) => StiValueHelper.TryToString(c));
	}

	public static decimal? FirstOrDefaultAsNullableDecimal<TSource>(this IEnumerable<TSource> source)
	{
		return StiValueHelper.TryToNullableDecimal(GetValueOrFirstOrDefault(source.FirstOrDefault()));
	}

	public static double? FirstOrDefaultAsNullableDouble<TSource>(this IEnumerable<TSource> source)
	{
		return StiValueHelper.TryToNullableDouble(GetValueOrFirstOrDefault(source.FirstOrDefault()));
	}

	public static decimal FirstOrDefaultAsDecimal<TSource>(this IEnumerable<TSource> source)
	{
		return StiValueHelper.TryToDecimal(GetValueOrFirstOrDefault(source.FirstOrDefault()));
	}

	public static object GetValueOrFirstOrDefault(object value)
	{
		if (!ListExt.IsList(value))
		{
			return value;
		}
		return ListExt.ToList(value).FirstOrDefault();
	}

	public static IEnumerable<object> Add(object a, object b)
	{
		IEnumerable<object> enumerable = a as IEnumerable<object>;
		IEnumerable<object> enumerable2 = b as IEnumerable<object>;
		if (enumerable == null && enumerable2 == null)
		{
			return null;
		}
		if (enumerable != null && enumerable2 != null)
		{
			return enumerable.Zip(enumerable2, (object v1, object v2) => StiValueHelper.TryToDecimal(v1) + StiValueHelper.TryToDecimal(v2)).Cast<object>();
		}
		if (enumerable != null)
		{
			decimal bb = StiValueHelper.TryToDecimal(b);
			return enumerable.Select((object v) => StiValueHelper.TryToDecimal(v) + bb).Cast<object>();
		}
		decimal aa = StiValueHelper.TryToDecimal(a);
		return enumerable2.Select((object v) => StiValueHelper.TryToDecimal(v) + aa).Cast<object>();
	}

	public static IEnumerable<object> Sub(object a, object b)
	{
		IEnumerable<object> enumerable = a as IEnumerable<object>;
		IEnumerable<object> enumerable2 = b as IEnumerable<object>;
		if (enumerable == null && enumerable2 == null)
		{
			return null;
		}
		if (enumerable != null && enumerable2 != null)
		{
			return enumerable.Zip(enumerable2, (object v1, object v2) => StiValueHelper.TryToDecimal(v1) - StiValueHelper.TryToDecimal(v2)).Cast<object>();
		}
		if (enumerable != null)
		{
			decimal bb = StiValueHelper.TryToDecimal(b);
			return enumerable.Select((object v) => StiValueHelper.TryToDecimal(v) - bb).Cast<object>();
		}
		decimal aa = StiValueHelper.TryToDecimal(a);
		return enumerable2.Select((object v) => StiValueHelper.TryToDecimal(v) - aa).Cast<object>();
	}

	public static IEnumerable<object> Mult(object a, object b)
	{
		IEnumerable<object> enumerable = a as IEnumerable<object>;
		IEnumerable<object> enumerable2 = b as IEnumerable<object>;
		if (enumerable == null && enumerable2 == null)
		{
			return null;
		}
		if (enumerable != null && enumerable2 != null)
		{
			return enumerable.Zip(enumerable2, (object v1, object v2) => StiValueHelper.TryToDecimal(v1) * StiValueHelper.TryToDecimal(v2)).Cast<object>();
		}
		if (enumerable != null)
		{
			decimal bb = StiValueHelper.TryToDecimal(b);
			return enumerable.Select((object v) => StiValueHelper.TryToDecimal(v) * bb).Cast<object>();
		}
		decimal aa = StiValueHelper.TryToDecimal(a);
		return enumerable2.Select((object v) => StiValueHelper.TryToDecimal(v) * aa).Cast<object>();
	}

	public static IEnumerable<object> BitwiseAnd(object a, object b)
	{
		IEnumerable<object> enumerable = a as IEnumerable<object>;
		IEnumerable<object> enumerable2 = b as IEnumerable<object>;
		if (enumerable == null && enumerable2 == null)
		{
			return null;
		}
		if (enumerable != null && enumerable2 != null)
		{
			IEnumerable<object> enumerable3 = enumerable.Zip(enumerable2, (object v1, object v2) => StiValueHelper.TryToInt(v1) & StiValueHelper.TryToInt(v2)).Cast<object>();
			if (ListExt.IsBoolList(enumerable))
			{
				return (from c in enumerable3.AsEnumerable().TryCastToInt()
					select c == 1).Cast<object>();
			}
			return enumerable3;
		}
		if (enumerable != null)
		{
			int bb = StiValueHelper.TryToInt(b);
			IEnumerable<object> enumerable4 = enumerable.Select((object v) => StiValueHelper.TryToInt(v) & bb).Cast<object>();
			if (ListExt.IsBoolList(enumerable))
			{
				return (from c in enumerable4.AsEnumerable().TryCastToInt()
					select c == 1).Cast<object>();
			}
			return enumerable4;
		}
		int aa = StiValueHelper.TryToInt(a);
		IEnumerable<object> enumerable5 = enumerable2.Select((object v) => StiValueHelper.TryToInt(v) & aa).Cast<object>();
		if (ListExt.IsBoolList(enumerable2))
		{
			return (from c in enumerable5.AsEnumerable().TryCastToInt()
				select c == 1).Cast<object>();
		}
		return enumerable5;
	}

	public static IEnumerable<object> BitwiseXOr(object a, object b)
	{
		IEnumerable<object> enumerable = a as IEnumerable<object>;
		IEnumerable<object> enumerable2 = b as IEnumerable<object>;
		if (enumerable == null && enumerable2 == null)
		{
			return null;
		}
		if (enumerable != null && enumerable2 != null)
		{
			IEnumerable<object> enumerable3 = enumerable.Zip(enumerable2, (object v1, object v2) => StiValueHelper.TryToInt(v1) ^ StiValueHelper.TryToInt(v2)).Cast<object>();
			if (ListExt.IsBoolList(enumerable))
			{
				return (from c in enumerable3.AsEnumerable().TryCastToInt()
					select c == 1).Cast<object>();
			}
			return enumerable3;
		}
		if (enumerable != null)
		{
			int bb = StiValueHelper.TryToInt(b);
			IEnumerable<object> enumerable4 = enumerable.Select((object v) => StiValueHelper.TryToInt(v) ^ bb).Cast<object>();
			if (ListExt.IsBoolList(enumerable))
			{
				return (from c in enumerable4.AsEnumerable().TryCastToInt()
					select c == 1).Cast<object>();
			}
			return enumerable4;
		}
		int aa = StiValueHelper.TryToInt(a);
		IEnumerable<object> enumerable5 = enumerable2.Select((object v) => StiValueHelper.TryToInt(v) ^ aa).Cast<object>();
		if (ListExt.IsBoolList(enumerable2))
		{
			return (from c in enumerable5.AsEnumerable().TryCastToInt()
				select c == 1).Cast<object>();
		}
		return enumerable5;
	}

	public static IEnumerable<object> BitwiseOr(object a, object b)
	{
		IEnumerable<object> enumerable = a as IEnumerable<object>;
		IEnumerable<object> enumerable2 = b as IEnumerable<object>;
		if (enumerable == null && enumerable2 == null)
		{
			return null;
		}
		if (enumerable != null && enumerable2 != null)
		{
			IEnumerable<object> enumerable3 = enumerable.Zip(enumerable2, (object v1, object v2) => StiValueHelper.TryToInt(v1) | StiValueHelper.TryToInt(v2)).Cast<object>();
			if (ListExt.IsBoolList(enumerable))
			{
				return (from c in enumerable3.AsEnumerable().TryCastToInt()
					select c == 1).Cast<object>();
			}
			return enumerable3;
		}
		if (enumerable != null)
		{
			int bb = StiValueHelper.TryToInt(b);
			IEnumerable<object> enumerable4 = enumerable.Select((object v) => StiValueHelper.TryToInt(v) | bb).Cast<object>();
			if (ListExt.IsBoolList(enumerable))
			{
				return (from c in enumerable4.AsEnumerable().TryCastToInt()
					select c == 1).Cast<object>();
			}
			return enumerable4;
		}
		int aa = StiValueHelper.TryToInt(a);
		IEnumerable<object> enumerable5 = enumerable2.Select((object v) => StiValueHelper.TryToInt(v) | aa).Cast<object>();
		if (ListExt.IsBoolList(enumerable2))
		{
			return (from c in enumerable5.AsEnumerable().TryCastToInt()
				select c == 1).Cast<object>();
		}
		return enumerable5;
	}

	public static IEnumerable<object> Div(object a, object b)
	{
		IEnumerable<object> enumerable = a as IEnumerable<object>;
		IEnumerable<object> enumerable2 = b as IEnumerable<object>;
		if (enumerable == null && enumerable2 == null)
		{
			return null;
		}
		if (enumerable != null && enumerable2 != null)
		{
			return enumerable.Zip(enumerable2, (object v1, object v2) => (!(StiValueHelper.TryToDecimal(v2) != 0m)) ? 0m : (StiValueHelper.TryToDecimal(v1) / StiValueHelper.TryToDecimal(v2))).Cast<object>();
		}
		if (enumerable != null)
		{
			decimal bb = StiValueHelper.TryToDecimal(b);
			return enumerable.Select((object v) => (!(bb != 0m)) ? 0m : (StiValueHelper.TryToDecimal(v) / bb)).Cast<object>();
		}
		decimal aa = StiValueHelper.TryToDecimal(a);
		return enumerable2.Select((object v) => (!(aa != 0m)) ? 0m : (StiValueHelper.TryToDecimal(v) / aa)).Cast<object>();
	}
}
