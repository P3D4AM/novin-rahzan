using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Map;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Data.Options;
using Stimulsoft.Data.Types;

namespace Stimulsoft.Data.Functions;

public static class Funcs
{
	private static int[] Arabics = new int[6] { 1, 5, 10, 50, 100, 1000 };

	private static char[] Romans = new char[6] { 'I', 'V', 'X', 'L', 'C', 'M' };

	private static int[] Subs = new int[6] { 0, 0, 0, 2, 2, 4 };

	private static char[] ABC = new char[26]
	{
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
		'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
		'U', 'V', 'W', 'X', 'Y', 'Z'
	};

	private static char[] ABCRu = new char[28]
	{
		'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ж', 'З', 'И', 'К',
		'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф',
		'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Э', 'Ю', 'Я'
	};

	private static Dictionary<string, string> toProperCaseCache = new Dictionary<string, string>();

	private static Dictionary<string, string> toLowerCaseCache = new Dictionary<string, string>();

	private static Dictionary<string, string> toUpperCaseCache = new Dictionary<string, string>();

	private static Dictionary<string, string> toDataNameCache = new Dictionary<string, string>();

	private static Dictionary<string, List<string>> iso2Cache = new Dictionary<string, List<string>>();

	public static decimal Avg(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDecimal(value);
		}
		IEnumerable<object> value2 = SkipNulls(ListExt.ToList(value));
		if (Count(value2) == 0L)
		{
			return 0m;
		}
		return Sum(value2) / (decimal)Count(value2);
	}

	public static decimal? AvgNulls(object value)
	{
		if (!ListExt.IsList(value))
		{
			if (!IsNull(value))
			{
				return StiValueHelper.TryToDecimal(value);
			}
			return null;
		}
		IEnumerable<object> enumerable = ListExt.ToList(value);
		if (enumerable == null || enumerable.Count() == 0)
		{
			return null;
		}
		return Sum(enumerable) / (decimal)enumerable.Count();
	}

	public static double AvgD(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDouble(value);
		}
		IEnumerable<object> value2 = SkipNulls(ListExt.ToList(value));
		if (Count(value2) == 0L)
		{
			return 0.0;
		}
		return SumD(value2) / (double)Count(value2);
	}

	public static long AvgI(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToLong(value);
		}
		IEnumerable<object> value2 = SkipNulls(ListExt.ToList(value));
		if (Count(value2) == 0L)
		{
			return 0L;
		}
		return SumI(value2) / Count(value2);
	}

	public static DateTime? AvgDate(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToNullableDateTime(value);
		}
		IEnumerable<DateTime> enumerable = (from d in SkipNulls(ListExt.ToList(value))
			where d != null && d is DateTime
			select d).TryCastToDateTime();
		if (Count(enumerable) == 0L)
		{
			return null;
		}
		return new DateTime(enumerable.Sum((DateTime d) => d.Ticks) / Count(enumerable));
	}

	public static TimeSpan? AvgTime(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToNullableTimeSpan(value);
		}
		IEnumerable<TimeSpan> enumerable = (from d in SkipNulls(ListExt.ToList(value))
			select GetTimeSpan(d) into d
			where d.HasValue
			select d).TryCastToTimeSpan();
		if (Count(enumerable) == 0L)
		{
			return null;
		}
		return TimeSpan.FromTicks(Convert.ToInt64(enumerable.Average((TimeSpan d) => d.Ticks)));
	}

	private static TimeSpan? GetTimeSpan(object value)
	{
		if (value is DateTime dateTime)
		{
			return dateTime.TimeOfDay;
		}
		if (value is TimeSpan)
		{
			return (TimeSpan)value;
		}
		return null;
	}

	public static long Count(object value)
	{
		if (ListExt.IsList(value))
		{
			return OptionalSkipNulls(ListExt.ToList(value)).Count();
		}
		return StiValueHelper.TryToLong(value);
	}

	public static long CountIf(object value, object condition)
	{
		if (!ListExt.IsList(value))
		{
			if (!GetCondition(condition))
			{
				return 0L;
			}
			return 1L;
		}
		IEnumerable<object> enumerable = SkipNulls(ListExt.ToList(value));
		List<bool> conditions = GetConditions(condition);
		long num = 0L;
		int num2 = 0;
		foreach (object item in enumerable)
		{
			if ((num2 < conditions.Count) ? conditions[num2] : conditions.LastOrDefault())
			{
				num++;
			}
			num2++;
		}
		return num;
	}

	public static object Distinct(object value)
	{
		if (ListExt.IsList(value))
		{
			return OptionalSkipNulls(ListExt.ToList(value)).Distinct();
		}
		return value;
	}

	public static long DistinctCount(object value)
	{
		if (ListExt.IsList(value))
		{
			return OptionalSkipNulls(ListExt.ToList(value)).Distinct().Count();
		}
		return 1L;
	}

	public static long DistinctCountIf(object value, object condition)
	{
		if (!ListExt.IsList(value))
		{
			if (!GetCondition(condition))
			{
				return 0L;
			}
			return 1L;
		}
		IEnumerable<object> enumerable = ListExt.ToList(value);
		List<bool> conditions = GetConditions(condition);
		Hashtable hashtable = new Hashtable();
		long num = 0L;
		int num2 = 0;
		foreach (object item in enumerable)
		{
			if (((num2 < conditions.Count) ? conditions[num2] : conditions.LastOrDefault()) && hashtable[item] == null)
			{
				num++;
			}
			hashtable[item] = item;
			num2++;
		}
		return num;
	}

	public static object First(object value)
	{
		if (ListExt.IsList(value))
		{
			return SkipNulls(ListExt.ToList(value)).FirstOrDefault();
		}
		return value;
	}

	public static object Last(object value)
	{
		if (ListExt.IsList(value))
		{
			return SkipNulls(ListExt.ToList(value)).LastOrDefault();
		}
		return value;
	}

	public static object All(object value)
	{
		if (ListExt.IsList(value))
		{
			return from v in SkipNulls(ListExt.ToList(value))
				select new SimpleValue(v);
		}
		return new SimpleValue(value);
	}

	public static IEnumerable<object> RunningTotal(IEnumerable<object> values)
	{
		IEnumerable<decimal> enumerable = values.Select((object v) => (!(v is IEnumerable<object>)) ? StiValueHelper.TryToDecimal(v) : Sum(v as IEnumerable<object>));
		decimal start = default(decimal);
		foreach (decimal item in enumerable)
		{
			yield return start + item;
			start += item;
		}
	}

	public static bool IsAggregationFunction(string function)
	{
		switch (ToLowerCase(function))
		{
		case "avg":
		case "avgd":
		case "avgi":
		case "avgdate":
		case "avgtime":
		case "max":
		case "maxd":
		case "maxi":
		case "maxdate":
		case "maxtime":
		case "maxstr":
		case "median":
		case "mediand":
		case "mediani":
		case "min":
		case "mind":
		case "mini":
		case "mindate":
		case "mintime":
		case "minstr":
		case "minmaxdatestring":
		case "mode":
		case "moded":
		case "modei":
		case "sum":
		case "sumd":
		case "sumi":
		case "sumtime":
		case "sumdistinct":
		case "sumif":
		case "sumdif":
		case "sumiif":
		case "sumtimeif":
		case "sumdistinctif":
		case "count":
		case "countdistinct":
		case "countif":
		case "countdistinctif":
			return true;
		default:
			return false;
		}
	}

	public static decimal Max(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDecimal(value);
		}
		IEnumerable<decimal> source = SkipNulls(ListExt.ToList(value)).TryCastToDecimal();
		if (!source.Any())
		{
			return 0m;
		}
		return source.Max();
	}

	public static decimal? MaxNulls(object value)
	{
		if (!ListExt.IsList(value))
		{
			if (!IsNull(value))
			{
				return StiValueHelper.TryToDecimal(value);
			}
			return null;
		}
		IEnumerable<object> enumerable = ListExt.ToList(value);
		if (enumerable == null || enumerable.Count() == 0 || enumerable.All((object v) => IsNull(v)))
		{
			return null;
		}
		IEnumerable<decimal> source = SkipNulls(enumerable).TryCastToDecimal();
		return source.Any() ? source.Max() : 0m;
	}

	public static double MaxD(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDouble(value);
		}
		IEnumerable<double> source = SkipNulls(ListExt.ToList(value)).TryCastToDouble();
		if (!source.Any())
		{
			return 0.0;
		}
		return source.Max();
	}

	public static long MaxI(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToLong(value);
		}
		IEnumerable<long> source = SkipNulls(ListExt.ToList(value)).TryCastToLong();
		if (!source.Any())
		{
			return 0L;
		}
		return source.Max();
	}

	public static DateTime? MaxDate(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToNullableDateTime(value);
		}
		IEnumerable<DateTime?> source = SkipNulls(ListExt.ToList(value)).TryCastToNullableDateTime();
		if (!source.Any())
		{
			return null;
		}
		return source.Max();
	}

	public static TimeSpan? MaxTime(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToNullableTimeSpan(value);
		}
		IEnumerable<TimeSpan?> source = SkipNulls(ListExt.ToList(value)).TryCastToNullableTimeSpan();
		if (!source.Any())
		{
			return null;
		}
		return source.Max();
	}

	public static string MaxStr(object value)
	{
		if (!ListExt.IsList(value))
		{
			return ToString(value);
		}
		return SkipNulls(ListExt.ToList(value)).OrderBy(ToString).Cast<string>().LastOrDefault();
	}

	public static decimal Median(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDecimal(value);
		}
		decimal[] array = (from n in ListExt.ToList(value).TryCastToDecimal()
			orderby n
			select n).ToArray();
		if (!array.Any())
		{
			return 0m;
		}
		if (array.Length == 1)
		{
			return array[0];
		}
		if (array.Length % 2 == 0)
		{
			return (array[array.Length / 2 - 1] + array[array.Length / 2]) / 2.0m;
		}
		return array[array.Length / 2];
	}

	public static double MedianD(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDouble(value);
		}
		double[] array = (from n in ListExt.ToList(value).TryCastToDouble()
			orderby n
			select n).ToArray();
		if (!array.Any())
		{
			return 0.0;
		}
		if (array.Length == 1)
		{
			return array[0];
		}
		if (array.Length % 2 == 0)
		{
			return (array[array.Length / 2 - 1] + array[array.Length / 2]) / 2.0;
		}
		return array[array.Length / 2];
	}

	public static long MedianI(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToLong(value);
		}
		long[] array = (from n in ListExt.ToList(value).TryCastToLong()
			orderby n
			select n).ToArray();
		if (!array.Any())
		{
			return 0L;
		}
		if (array.Length == 1)
		{
			return array[0];
		}
		if (array.Length % 2 == 0)
		{
			return (array[array.Length / 2 - 1] + array[array.Length / 2]) / 2;
		}
		return array[array.Length / 2];
	}

	public static decimal Min(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDecimal(value);
		}
		IEnumerable<decimal> source = SkipNulls(ListExt.ToList(value)).TryCastToDecimal();
		if (!source.Any())
		{
			return 0m;
		}
		return source.Min();
	}

	public static decimal? MinNulls(object value)
	{
		if (!ListExt.IsList(value))
		{
			if (!IsNull(value))
			{
				return StiValueHelper.TryToDecimal(value);
			}
			return null;
		}
		IEnumerable<object> enumerable = ListExt.ToList(value);
		if (enumerable == null || enumerable.Count() == 0 || enumerable.All((object v) => IsNull(v)))
		{
			return null;
		}
		IEnumerable<decimal> source = SkipNulls(enumerable).TryCastToDecimal();
		return source.Any() ? source.Min() : 0m;
	}

	public static double MinD(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDouble(value);
		}
		IEnumerable<double> source = SkipNulls(ListExt.ToList(value)).TryCastToDouble();
		if (!source.Any())
		{
			return 0.0;
		}
		return source.Min();
	}

	public static long MinI(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToLong(value);
		}
		IEnumerable<long> source = SkipNulls(ListExt.ToList(value)).TryCastToLong();
		if (!source.Any())
		{
			return 0L;
		}
		return source.Min();
	}

	public static DateTime? MinDate(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToNullableDateTime(value);
		}
		IEnumerable<DateTime?> source = SkipNulls(ListExt.ToList(value)).TryCastToNullableDateTime();
		if (!source.Any())
		{
			return null;
		}
		return source.Min();
	}

	public static TimeSpan? MinTime(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToNullableTimeSpan(value);
		}
		IEnumerable<TimeSpan?> source = SkipNulls(ListExt.ToList(value)).TryCastToNullableTimeSpan();
		if (!source.Any())
		{
			return null;
		}
		return source.Min();
	}

	public static string MinMaxDateString(object value)
	{
		DateTime? dateTime = MinDate(value);
		DateTime? dateTime2 = MaxDate(value);
		string text = (dateTime.HasValue ? dateTime.Value.ToString("MM'/'dd'/'yyyy") : "null");
		string text2 = (dateTime2.HasValue ? dateTime2.Value.ToString("MM'/'dd'/'yyyy") : "null");
		return text + "-" + text2;
	}

	public static string MinStr(object value)
	{
		if (!ListExt.IsList(value))
		{
			return ToString(value);
		}
		return SkipNulls(ListExt.ToList(value)).OrderBy(ToString).Cast<string>().FirstOrDefault();
	}

	public static decimal Mode(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDecimal(value);
		}
		return (from n in SkipNulls(ListExt.ToList(value)).TryCastToDecimal()
			group n by n into g
			orderby g.Count() descending
			select g.Key).FirstOrDefault();
	}

	public static double ModeD(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDouble(value);
		}
		return (from n in SkipNulls(ListExt.ToList(value)).TryCastToDouble()
			group n by n into g
			orderby g.Count() descending
			select g.Key).FirstOrDefault();
	}

	public static long ModeI(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToLong(value);
		}
		return (from n in SkipNulls(ListExt.ToList(value)).TryCastToLong()
			group n by n into g
			orderby g.Count() descending
			select g.Key).FirstOrDefault();
	}

	public static decimal Sum(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDecimal(value);
		}
		return SkipNulls(ListExt.ToList(value)).TryCastToDecimal().Sum();
	}

	public static decimal? SumNulls(object value)
	{
		if (!ListExt.IsList(value))
		{
			if (!IsNull(value))
			{
				return StiValueHelper.TryToDecimal(value);
			}
			return null;
		}
		IEnumerable<object> enumerable = ListExt.ToList(value);
		if (enumerable.All((object v) => IsNull(v)))
		{
			return null;
		}
		return Sum(enumerable);
	}

	private static bool IsNull(object value)
	{
		if (value != null)
		{
			return value == DBNull.Value;
		}
		return true;
	}

	public static double SumD(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDouble(value);
		}
		return SkipNulls(ListExt.ToList(value)).TryCastToDouble().Sum();
	}

	public static long SumI(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToLong(value);
		}
		return SkipNulls(ListExt.ToList(value)).TryCastToLong().Sum();
	}

	public static TimeSpan SumTime(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToTimeSpan(value);
		}
		long ticks = SkipNulls(ListExt.ToList(value)).TryCastToTimeSpan().Sum((TimeSpan t) => t.Ticks);
		return new TimeSpan(ticks);
	}

	public static decimal SumDistinct(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToDecimal(value);
		}
		return SkipNulls(ListExt.ToList(value)).TryCastToDecimal().Distinct().Sum();
	}

	public static decimal SumIf(object value, object condition)
	{
		if (!ListExt.IsList(value))
		{
			if (!GetCondition(condition))
			{
				return 0m;
			}
			return StiValueHelper.TryToDecimal(value);
		}
		IEnumerable<decimal> enumerable = ListExt.ToList(value).TryCastToDecimal();
		List<bool> conditions = GetConditions(condition);
		int num = 0;
		decimal result = default(decimal);
		foreach (decimal item in enumerable)
		{
			if ((num < conditions.Count) ? conditions[num] : conditions.LastOrDefault())
			{
				result += item;
			}
			num++;
		}
		return result;
	}

	public static double SumDIf(object value, object condition)
	{
		if (!ListExt.IsList(value))
		{
			if (!GetCondition(condition))
			{
				return 0.0;
			}
			return StiValueHelper.TryToDouble(value);
		}
		IEnumerable<double> enumerable = ListExt.ToList(value).TryCastToDouble();
		List<bool> conditions = GetConditions(condition);
		int num = 0;
		double num2 = 0.0;
		foreach (double item in enumerable)
		{
			if ((num < conditions.Count) ? conditions[num] : conditions.LastOrDefault())
			{
				num2 += item;
			}
			num++;
		}
		return num2;
	}

	public static long SumIIf(object value, object condition)
	{
		if (!ListExt.IsList(value))
		{
			if (!GetCondition(condition))
			{
				return 0L;
			}
			return StiValueHelper.TryToLong(value);
		}
		IEnumerable<long> enumerable = ListExt.ToList(value).TryCastToLong();
		List<bool> conditions = GetConditions(condition);
		int num = 0;
		long num2 = 0L;
		foreach (long item in enumerable)
		{
			if ((num < conditions.Count) ? conditions[num] : conditions.LastOrDefault())
			{
				num2 += item;
			}
			num++;
		}
		return num2;
	}

	public static TimeSpan SumTimeIf(object value, object condition)
	{
		if (!ListExt.IsList(value))
		{
			if (!GetCondition(condition))
			{
				return TimeSpan.Zero;
			}
			return StiValueHelper.TryToTimeSpan(value);
		}
		IEnumerable<TimeSpan> enumerable = ListExt.ToList(value).TryCastToTimeSpan();
		List<bool> conditions = GetConditions(condition);
		int num = 0;
		long num2 = 0L;
		foreach (TimeSpan item in enumerable)
		{
			if ((num < conditions.Count) ? conditions[num] : conditions.LastOrDefault())
			{
				num2 += item.Ticks;
			}
			num++;
		}
		return new TimeSpan(num2);
	}

	public static decimal SumDistinctIf(object value, object condition)
	{
		if (!ListExt.IsList(value))
		{
			if (!GetCondition(condition))
			{
				return 0m;
			}
			return StiValueHelper.TryToDecimal(value);
		}
		IEnumerable<decimal> enumerable = ListExt.ToList(value).TryCastToDecimal();
		List<bool> conditions = GetConditions(condition);
		Hashtable hashtable = new Hashtable();
		int num = 0;
		decimal result = default(decimal);
		foreach (decimal item in enumerable)
		{
			bool flag = ((num < conditions.Count) ? conditions[num] : conditions.LastOrDefault());
			if (flag && hashtable[flag] != null)
			{
				result += item;
				hashtable[flag] = flag;
			}
			num++;
		}
		return result;
	}

	private static bool GetCondition(object value)
	{
		if (!ListExt.IsList(value))
		{
			return StiValueHelper.TryToBool(value);
		}
		return ListExt.ToList(value).TryCastToBool().FirstOrDefault();
	}

	private static List<bool> GetConditions(object value)
	{
		if (!ListExt.IsList(value))
		{
			return new List<bool> { StiValueHelper.TryToBool(value) };
		}
		return ListExt.ToList(value).TryCastToBool().ToList();
	}

	public static IEnumerable<object> SkipNulls(IEnumerable<object> values)
	{
		return values?.Where((object r) => r != DBNull.Value && r != null);
	}

	public static IEnumerable<object> OptionalSkipNulls(IEnumerable<object> values)
	{
		if (!Stimulsoft.Data.Options.StiDataOptions.AllowNulls)
		{
			return SkipNulls(values);
		}
		return values;
	}

	public static string ToRoman(int value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		while (value > 0)
		{
			for (int num = 5; num >= 0; num--)
			{
				if (value >= Arabics[num])
				{
					stringBuilder.Append(Romans[num]);
					value -= Arabics[num];
					break;
				}
				bool flag = false;
				for (int i = Subs[num]; i < num; i++)
				{
					if (Arabics[i] != Arabics[num] - Arabics[i] && value >= Arabics[num] - Arabics[i])
					{
						stringBuilder.Append(Romans[i]);
						stringBuilder.Append(Romans[num]);
						value -= Arabics[num] - Arabics[i];
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		return stringBuilder.ToString();
	}

	public static string ToABC(int value)
	{
		if (value < 1)
		{
			return string.Empty;
		}
		int num = 0;
		while (value > 26)
		{
			num++;
			value -= 26;
		}
		return new string(ABC[value - 1], num + 1);
	}

	public static string ToABCNumeric(int value)
	{
		int num = 0;
		while (value > 26)
		{
			num++;
			value -= 26;
		}
		if (num == 0)
		{
			return ((char)(value + 64)).ToString();
		}
		return ((char)(value + 64)).ToString() + num;
	}

	public static string ToABCRu(int value)
	{
		if (value < 1)
		{
			return string.Empty;
		}
		int num = 0;
		while (value > 26)
		{
			num++;
			value -= 26;
		}
		return new string(ABCRu[value - 1], num + 1);
	}

	public static string ToArabic(int value, bool useEasternDigits)
	{
		return ToArabic(value.ToString(), useEasternDigits);
	}

	public static string ToArabic(string value, bool useEasternDigits)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < value.Length; i++)
		{
			int num = value[i];
			if (num >= 48 && num <= 57)
			{
				num += 1584;
				if (useEasternDigits)
				{
					num += 144;
				}
			}
			stringBuilder.Append((char)num);
		}
		return stringBuilder.ToString();
	}

	public static bool ExistsCustomFunction(string funcName)
	{
		return GetCustomFunctions(funcName).Any();
	}

	public static IEnumerable<IStiAppFunction> GetCustomFunctions(string funcName)
	{
		IStiAppFunction[] functions = StiAppFunctions.GetFunctions(funcName, isCompile: true, isCaseSensitive: false);
		if (functions != null)
		{
			return functions;
		}
		return new List<IStiAppFunction>();
	}

	public static IStiAppFunction GetCustomFunction(string funcName, IEnumerable<Type> argumentTypes)
	{
		IEnumerable<IStiAppFunction> customFunctions = GetCustomFunctions(funcName);
		return customFunctions.FirstOrDefault();
	}

	public static object InvokeCustomFunction(string funcName, IEnumerable<object> arguments)
	{
		IEnumerable<IStiAppFunction> customFunctions = GetCustomFunctions(funcName);
		if (customFunctions.Count() == 1)
		{
			return customFunctions.FirstOrDefault().Invoke(arguments);
		}
		IStiAppFunction customFunction = GetCustomFunction(funcName, arguments.Select((object a) => a?.GetType()));
		if (customFunction == null)
		{
			throw new StiFunctionNotFoundException(funcName);
		}
		return customFunction.Invoke(arguments);
	}

	public static object AddMonthsObject(object date, int months)
	{
		DateTime? dateTime = StiValueHelper.TryToNullableDateTime(date);
		if (!dateTime.HasValue)
		{
			return null;
		}
		return dateTime.Value.AddMonths(months);
	}

	public static DateTime FromOADate(double value)
	{
		return System.DateTime.FromOADate(value);
	}

	public static object FromOADateObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(FromOADate);
		}
		return System.DateTime.FromOADate(StiValueHelper.TryToDouble(value));
	}

	public static double ToOADate(DateTime value)
	{
		return value.ToOADate();
	}

	public static object ToOADateObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return from c in ListExt.ToNullableDateTimeList(value)
				select c.HasValue ? c.Value.ToOADate() : 0.0;
		}
		return StiValueHelper.TryToDateTime(value).ToOADate();
	}

	public static DateTime AddYears(DateTime date, int years)
	{
		return date.AddYears(years);
	}

	public static object AddYearsObject(object date, int years)
	{
		DateTime? dateTime = StiValueHelper.TryToNullableDateTime(date);
		if (!dateTime.HasValue)
		{
			return null;
		}
		return AddYears(dateTime.Value, years);
	}

	public static int Day(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return dateTime.Value.Day;
	}

	public static object DayObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(Day);
		}
		return Day(StiValueHelper.TryToNullableDateTime(value));
	}

	public static TimeSpan? DateDiff(DateTime? date1, DateTime? date2)
	{
		if (!date1.HasValue || !date2.HasValue)
		{
			return null;
		}
		return date1.Value.Subtract(date2.Value);
	}

	public static object DateDiffObject(object value1, object value2)
	{
		if (ListExt.IsList(value1) || ListExt.IsList(value2))
		{
			IEnumerable<DateTime?> enumerable = ListExt.ToNullableDateTimeList(value1);
			IEnumerable<DateTime?> enumerable2 = ListExt.ToNullableDateTimeList(value2);
			List<DateTime?> list = enumerable?.ToList();
			List<DateTime?> list2 = enumerable2?.ToList();
			int num = list?.Count ?? list2.Count;
			List<TimeSpan?> list3 = new List<TimeSpan?>();
			for (int i = 0; i < num; i++)
			{
				DateTime? date = list?[i];
				DateTime? date2 = list2?[i];
				list3.Add(DateDiff(date, date2));
			}
			return list3;
		}
		return DateDiff(StiValueHelper.TryToNullableDateTime(value1), StiValueHelper.TryToNullableDateTime(value2));
	}

	public static object DateTime(object value)
	{
		if (ListExt.IsList(value))
		{
			return from v in SkipNulls(ListExt.ToList(value))
				select new DateTimeValue(v);
		}
		return new DateTimeValue(value);
	}

	public static int DayOfYear(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return dateTime.Value.DayOfYear;
	}

	public static object DayOfYearObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(DayOfYear);
		}
		return DayOfYear(StiValueHelper.TryToNullableDateTime(value));
	}

	public static StiQuarter? FinancialQuarter(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return null;
		}
		return (StiQuarter)Enum.ToObject(typeof(StiQuarter), FinancialQuarterIndex(dateTime));
	}

	public static object FinancialQuarterObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(FinancialQuarter);
		}
		return FinancialQuarter(StiValueHelper.TryToNullableDateTime(value));
	}

	public static int FinancialQuarterIndex(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return (int)(Math.Ceiling((double)dateTime.Value.Month / 3.0 + 2.0) % 4.0 + 1.0);
	}

	public static object FinancialQuarterIndexObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(FinancialQuarterIndex);
		}
		return FinancialQuarterIndex(StiValueHelper.TryToNullableDateTime(value));
	}

	public static int Hour(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return dateTime.Value.Hour;
	}

	public static object HourObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(Hour);
		}
		return Hour(StiValueHelper.TryToNullableDateTime(value));
	}

	public static DateTime MakeDate(int year, int month = 1, int day = 1)
	{
		return new DateTime(year, month, day);
	}

	public static object MakeDateObject(object year, object month = null, object day = null)
	{
		int year2 = StiValueHelper.TryToInt(year);
		int month2 = StiValueHelper.TryToNullableInt(month) ?? 1;
		int day2 = StiValueHelper.TryToNullableInt(day) ?? 1;
		return MakeDate(year2, month2, day2);
	}

	public static DateTime MakeDateTime(int year, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0)
	{
		return new DateTime(year, month, day, hour, minute, second);
	}

	public static object MakeDateTimeObject(object year, object month = null, object day = null, object hour = null, object minute = null, object second = null)
	{
		int year2 = StiValueHelper.TryToInt(year);
		int month2 = StiValueHelper.TryToNullableInt(month) ?? 1;
		int day2 = StiValueHelper.TryToNullableInt(day) ?? 1;
		int valueOrDefault = StiValueHelper.TryToNullableInt(hour).GetValueOrDefault();
		int valueOrDefault2 = StiValueHelper.TryToNullableInt(minute).GetValueOrDefault();
		int valueOrDefault3 = StiValueHelper.TryToNullableInt(second).GetValueOrDefault();
		return MakeDateTime(year2, month2, day2, valueOrDefault, valueOrDefault2, valueOrDefault3);
	}

	public static DateTime MakeTime(int hour, int minute = 0, int second = 0)
	{
		DateTime dateTime = Now();
		return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second);
	}

	public static object MakeTimeObject(object hour, object minute = null, object second = null)
	{
		int hour2 = StiValueHelper.TryToInt(hour);
		int valueOrDefault = StiValueHelper.TryToNullableInt(minute).GetValueOrDefault();
		int valueOrDefault2 = StiValueHelper.TryToNullableInt(second).GetValueOrDefault();
		return MakeTime(hour2, valueOrDefault, valueOrDefault2);
	}

	public static int Minute(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return dateTime.Value.Minute;
	}

	public static object MinuteObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(Minute);
		}
		return Minute(StiValueHelper.TryToNullableDateTime(value));
	}

	public static DateTime Now()
	{
		return System.DateTime.Now;
	}

	public static string QuarterName(DateTime? dateTime, bool localized = true)
	{
		switch (Quarter(dateTime))
		{
		case StiQuarter.Q1:
			if (!localized)
			{
				return "First Quarter";
			}
			return Loc.Get("DatePickerRanges", "FirstQuarter");
		case StiQuarter.Q2:
			if (!localized)
			{
				return "Second Quarter";
			}
			return Loc.Get("DatePickerRanges", "SecondQuarter");
		case StiQuarter.Q3:
			if (!localized)
			{
				return "Third Quarter";
			}
			return Loc.Get("DatePickerRanges", "ThirdQuarter");
		case StiQuarter.Q4:
			if (!localized)
			{
				return "FourthQuarter";
			}
			return Loc.Get("DatePickerRanges", "FourthQuarter");
		default:
			return string.Empty;
		}
	}

	public static object QuarterNameObject(object value, bool localized = true)
	{
		if (ListExt.IsList(value))
		{
			return from c in ListExt.ToNullableDateTimeList(value)
				select QuarterName(c, localized);
		}
		return QuarterName(StiValueHelper.TryToNullableDateTime(value), localized);
	}

	public static StiQuarter? Quarter(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return null;
		}
		return (StiQuarter)Enum.ToObject(typeof(StiQuarter), QuarterIndex(dateTime));
	}

	public static object QuarterObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(Quarter);
		}
		return Quarter(StiValueHelper.TryToNullableDateTime(value));
	}

	public static int QuarterIndex(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return (dateTime.Value.Month + 2) / 3;
	}

	public static object QuarterIndexObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(QuarterIndex);
		}
		return QuarterIndex(StiValueHelper.TryToNullableDateTime(value));
	}

	public static int Second(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return dateTime.Value.Second;
	}

	public static object SecondObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(Second);
		}
		return Second(StiValueHelper.TryToNullableDateTime(value));
	}

	public static object Time(object value)
	{
		if (ListExt.IsList(value))
		{
			return from v in SkipNulls(ListExt.ToList(value))
				select (!(v is DateTime dateTime2)) ? null : new TimeSpan?(dateTime2.TimeOfDay);
		}
		return (value is DateTime dateTime) ? new TimeSpan?(dateTime.TimeOfDay) : null;
	}

	public static int Year(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return dateTime.Value.Year;
	}

	public static object YearObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(Year);
		}
		return Year(StiValueHelper.TryToNullableDateTime(value));
	}

	public static string YearMonth(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return "";
		}
		return $"{dateTime.Value.Year}-{dateTime.Value.Month:00}";
	}

	public static object YearMonthObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(YearMonth);
		}
		return YearMonth(StiValueHelper.TryToNullableDateTime(value));
	}

	public static DayOfWeek? DayOfWeekIdent(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return null;
		}
		return (DayOfWeek)Enum.ToObject(typeof(DayOfWeek), dateTime.Value.DayOfWeek);
	}

	public static object DayOfWeekIdentObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToList(value).Select(DayOfWeekIdentObject);
		}
		if (value is string)
		{
			return StiDayOfWeekToStrHelper.DayOfWeek(value as string);
		}
		return DayOfWeekIdent(StiValueHelper.TryToNullableDateTime(value));
	}

	public static int DayOfWeekIndex(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return (int)dateTime.Value.DayOfWeek;
	}

	public static object DayOfWeekIndexObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToList(value).Select(DayOfWeekIndexObject);
		}
		if (value is string)
		{
			DayOfWeek? dayOfWeek = StiDayOfWeekToStrHelper.DayOfWeek(value as string);
			if (!dayOfWeek.HasValue)
			{
				return -1;
			}
			return (int)dayOfWeek.Value;
		}
		return DayOfWeekIndex(StiValueHelper.TryToNullableDateTime(value));
	}

	public static string DayOfWeek(DateTime? date)
	{
		if (!date.HasValue)
		{
			return string.Empty;
		}
		return StiDayOfWeekToStrHelper.DayOfWeek(date.Value);
	}

	public static object DayOfWeekObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(DayOfWeek);
		}
		return DayOfWeek(StiValueHelper.TryToNullableDateTime(value));
	}

	public static string DayOfWeek(DateTime? date, bool localized)
	{
		if (!date.HasValue)
		{
			return string.Empty;
		}
		return StiDayOfWeekToStrHelper.DayOfWeek(date.Value, localized);
	}

	public static object DayOfWeekObject(object value, bool localized)
	{
		if (ListExt.IsList(value))
		{
			return from d in ListExt.ToNullableDateTimeList(value)
				select DayOfWeek(d, localized);
		}
		return DayOfWeek(StiValueHelper.TryToNullableDateTime(value), localized);
	}

	public static string DayOfWeek(DateTime? date, string culture)
	{
		if (!date.HasValue)
		{
			return string.Empty;
		}
		return StiDayOfWeekToStrHelper.DayOfWeek(date.Value, culture);
	}

	public static object DayOfWeekObject(object value, string culture)
	{
		if (ListExt.IsList(value))
		{
			return from d in ListExt.ToNullableDateTimeList(value)
				select DayOfWeek(d, culture);
		}
		return DayOfWeek(StiValueHelper.TryToNullableDateTime(value), culture);
	}

	public static string DayOfWeek(DateTime? date, string culture, bool upperCase)
	{
		if (!date.HasValue)
		{
			return string.Empty;
		}
		return StiDayOfWeekToStrHelper.DayOfWeek(date.Value, culture, upperCase);
	}

	public static object DayOfWeekObject(object value, string culture, bool upperCase)
	{
		if (ListExt.IsList(value))
		{
			return from d in ListExt.ToNullableDateTimeList(value)
				select DayOfWeek(d, culture, upperCase);
		}
		return DayOfWeek(StiValueHelper.TryToNullableDateTime(value), culture, upperCase);
	}

	public static long DaysInMonth(long year, long month)
	{
		return System.DateTime.DaysInMonth((int)year, (int)month);
	}

	public static object DaysInMonthObject(object value1, object value2)
	{
		if (ListExt.IsList(value1) || ListExt.IsList(value2))
		{
			IEnumerable<DateTime?> enumerable = ListExt.ToNullableDateTimeList(value1);
			IEnumerable<DateTime?> enumerable2 = ListExt.ToNullableDateTimeList(value2);
			List<DateTime?> list = enumerable?.ToList();
			List<DateTime?> list2 = enumerable2?.ToList();
			int num = list?.Count ?? list2.Count;
			List<object> list3 = new List<object>();
			for (int i = 0; i < num; i++)
			{
				DateTime? dateTime = list?[i];
				DateTime? dateTime2 = list2?[i];
				list3.Add(DaysInMonthObject(dateTime, dateTime2));
			}
			return list3;
		}
		return DaysInMonth(StiValueHelper.TryToInt(value1), StiValueHelper.TryToInt(value2));
	}

	public static long DaysInMonth(DateTime? date)
	{
		if (date.HasValue)
		{
			return System.DateTime.DaysInMonth(date.Value.Year, date.Value.Month);
		}
		return 0L;
	}

	public static object DaysInMonthObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(DaysInMonth);
		}
		return DaysInMonth(StiValueHelper.TryToNullableDateTime(value));
	}

	public static int DaysInYear(long year)
	{
		if (!System.DateTime.IsLeapYear((int)year))
		{
			return 365;
		}
		return 366;
	}

	public static int DaysInYear(DateTime? date)
	{
		if (date.HasValue)
		{
			return DaysInYear(date.Value.Year);
		}
		return 0;
	}

	public static object DaysInYearObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(DaysInMonth);
		}
		if (value is DateTime)
		{
			return DaysInYear(StiValueHelper.TryToNullableDateTime(value));
		}
		return DaysInYear(StiValueHelper.TryToInt(value));
	}

	public static StiMonth? MonthIdent(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return null;
		}
		return (StiMonth)Enum.ToObject(typeof(StiMonth), Month(dateTime));
	}

	public static object MonthIdentObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToList(value).Select(MonthIdentObject);
		}
		if (value is string)
		{
			return StiMonthToStrHelper.Month(value as string);
		}
		if (value != null && value.GetType().IsNumericType())
		{
			return StiMonthToStrHelper.Month(StiValueHelper.TryToInt(value));
		}
		return MonthIdent(StiValueHelper.TryToNullableDateTime(value));
	}

	public static int Month(DateTime? dateTime)
	{
		if (!dateTime.HasValue)
		{
			return -1;
		}
		return dateTime.Value.Month;
	}

	public static object MonthObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToList(value).Select(MonthObject);
		}
		if (value is string)
		{
			StiMonth? stiMonth = StiMonthToStrHelper.Month(value as string);
			if (!stiMonth.HasValue)
			{
				return -1;
			}
			return (int)stiMonth.Value;
		}
		return Month(StiValueHelper.TryToNullableDateTime(value));
	}

	public static string MonthName(DateTime? date)
	{
		if (date.HasValue)
		{
			return StiMonthToStrHelper.MonthName(date.Value);
		}
		return string.Empty;
	}

	public static object MonthNameObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToNullableDateTimeList(value).Select(MonthName);
		}
		return MonthName(StiValueHelper.TryToNullableDateTime(value));
	}

	public static string MonthName(DateTime? date, bool localized)
	{
		if (date.HasValue)
		{
			return StiMonthToStrHelper.MonthName(date.Value, localized);
		}
		return string.Empty;
	}

	public static object MonthNameObject(object value, bool localized)
	{
		if (ListExt.IsList(value))
		{
			return from d in ListExt.ToNullableDateTimeList(value)
				select MonthName(d, localized);
		}
		return MonthName(StiValueHelper.TryToNullableDateTime(value), localized);
	}

	public static string MonthName(DateTime? date, string culture)
	{
		if (date.HasValue)
		{
			return StiMonthToStrHelper.MonthName(date.Value, culture);
		}
		return string.Empty;
	}

	public static object MonthNameObject(object value, string culture)
	{
		if (ListExt.IsList(value))
		{
			return from d in ListExt.ToNullableDateTimeList(value)
				select MonthName(d, culture);
		}
		return MonthName(StiValueHelper.TryToNullableDateTime(value), culture);
	}

	public static string MonthName(DateTime? date, string culture, bool upperCase)
	{
		if (date.HasValue)
		{
			return StiMonthToStrHelper.MonthName(date.Value, culture, upperCase);
		}
		return string.Empty;
	}

	public static object MonthNameObject(object value, string culture, bool upperCase)
	{
		if (ListExt.IsList(value))
		{
			return from d in ListExt.ToNullableDateTimeList(value)
				select MonthName(d, culture, upperCase);
		}
		return MonthName(StiValueHelper.TryToNullableDateTime(value), culture, upperCase);
	}

	public static IEnumerable<string> GetDateDimensionFunctions()
	{
		return new string[6] { "Year", "Quarter", "Month", "Day", "DateTime", "Time" };
	}

	public static object Image(object value, int width = 200, int height = 200)
	{
		if (!ListExt.IsList(value))
		{
			string text = value as string;
			if (!string.IsNullOrWhiteSpace(text) && IsValidUrl(text))
			{
				byte[] bytes = StiBytesFromURL.TryLoad(text);
				Image image = StiImageConverter.BytesToImage(bytes, width, height);
				return StiImageConverter.ImageToBytes(image, allowNulls: true);
			}
		}
		return from v in ListExt.ToList(value)
			select Image(v, width, height);
	}

	public static bool IsValidUrl(string hyperlink)
	{
		if (Uri.TryCreate(hyperlink, UriKind.Absolute, out Uri result))
		{
			if (!(result.Scheme == Uri.UriSchemeHttp) && !(result.Scheme == Uri.UriSchemeHttps) && !(result.Scheme == Uri.UriSchemeFtp))
			{
				return result.Scheme == Uri.UriSchemeMailto;
			}
			return true;
		}
		return false;
	}

	public static string Localize(string func)
	{
		return ToLowerCase(func) switch
		{
			"avg" => "Average", 
			"count" => "Count", 
			"distinctcount" => "Count (Distinct)", 
			"max" => "Max", 
			"median" => "Median", 
			"min" => "Min", 
			"sum" => "Sum", 
			"percentofgrandtotal" => "Percent of Grand Total", 
			_ => func, 
		};
	}

	public static decimal Abs(decimal value)
	{
		return Math.Abs(value);
	}

	public static object AbsObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDecimalList(value).Select(Abs);
		}
		return Abs(StiValueHelper.TryToDecimal(value));
	}

	public static double Acos(double value)
	{
		return Math.Acos(value);
	}

	public static object AcosObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(Acos);
		}
		return Acos(StiValueHelper.TryToDouble(value));
	}

	public static double Asin(double value)
	{
		return Math.Asin(value);
	}

	public static object AsinObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(Asin);
		}
		return Asin(StiValueHelper.TryToDouble(value));
	}

	public static double Atan(double value)
	{
		return Math.Atan(value);
	}

	public static object AtanObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(Atan);
		}
		return Atan(StiValueHelper.TryToDouble(value));
	}

	public static decimal Ceiling(decimal value)
	{
		return Math.Ceiling(value);
	}

	public static object CeilingObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDecimalList(value).Select(Ceiling);
		}
		return Ceiling(StiValueHelper.TryToDecimal(value));
	}

	public static double Cos(double value)
	{
		return Math.Cos(value);
	}

	public static object CosObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(Cos);
		}
		return Cos(StiValueHelper.TryToDouble(value));
	}

	public static decimal? Div(decimal value1, decimal value2, decimal? zeroResult = null)
	{
		if (value2 == 0m)
		{
			return zeroResult;
		}
		return Math.Truncate(value1 / value2);
	}

	public static object DivObject(object value1, object value2, object zeroResult = null)
	{
		decimal value3 = StiValueHelper.TryToDecimal(value1);
		decimal value4 = StiValueHelper.TryToDecimal(value2);
		decimal? zeroResult2 = StiValueHelper.TryToNullableDecimal(zeroResult);
		return Div(value3, value4, zeroResult2);
	}

	public static double Exp(double value)
	{
		return Math.Exp(value);
	}

	public static object ExpObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(Exp);
		}
		return Exp(StiValueHelper.TryToDouble(value));
	}

	public static decimal Floor(decimal value)
	{
		return Math.Floor(value);
	}

	public static object FloorObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDecimalList(value).Select(Floor);
		}
		return Floor(StiValueHelper.TryToDecimal(value));
	}

	public static double Log(double value)
	{
		return Math.Log(value);
	}

	public static object LogObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(Log);
		}
		return Log(StiValueHelper.TryToDouble(value));
	}

	public static decimal Round(decimal value, int decimals = 0)
	{
		return Math.Round(value, decimals, Stimulsoft.Data.Options.StiDataOptions.RoundType);
	}

	public static object RoundObject(object value, int decimals = 0)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToDecimalList(value)
				select Round(v, decimals);
		}
		return Round(StiValueHelper.TryToDecimal(value), decimals);
	}

	public static int Sign(decimal value)
	{
		return Math.Sign(value);
	}

	public static object SignObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDecimalList(value).Select(Sign);
		}
		return Sign(StiValueHelper.TryToDecimal(value));
	}

	public static double Sin(double value)
	{
		return Math.Sin(value);
	}

	public static object SinObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(Sin);
		}
		return Sin(StiValueHelper.TryToDouble(value));
	}

	public static double Sqrt(double value)
	{
		return Math.Sqrt(value);
	}

	public static object SqrtObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(Sqrt);
		}
		return Sqrt(StiValueHelper.TryToDouble(value));
	}

	public static double Tan(double value)
	{
		return Math.Tan(value);
	}

	public static object TanObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDoubleList(value).Select(Tan);
		}
		return Tan(StiValueHelper.TryToDouble(value));
	}

	public static decimal Truncate(decimal value)
	{
		return Math.Truncate(value);
	}

	public static object TruncateObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDecimalList(value).Select(Truncate);
		}
		return Truncate(StiValueHelper.TryToDecimal(value));
	}

	public static bool IsMeasureFunction(string expression)
	{
		string func = StiExpressionHelper.GetFunction(expression);
		if (func == null)
		{
			return false;
		}
		func = ToLowerCase(func);
		if (func == "percentofgrandtotal")
		{
			return true;
		}
		return GetMeasureFunctions().Any((string f) => ToLowerCase(f.ToString()) == func);
	}

	public static IEnumerable<string> GetMeasureFunctions()
	{
		return GetAggregateMeasureFunctions().Concat(GetCommonMeasureFunctions()).Distinct();
	}

	public static IEnumerable<string> GetAggregateMeasureFunctions()
	{
		return new string[7] { "Sum", "Avg", "Min", "Max", "DistinctCount", "Count", "Median" };
	}

	public static IEnumerable<string> GetCommonMeasureFunctions()
	{
		return new string[4] { "First", "Last", "Count", "DistinctCount" };
	}

	public static object Calculate(string function, IEnumerable<object> values)
	{
		return ToLowerCase(function) switch
		{
			"all" => All(values), 
			"avg" => Avg(values), 
			"avgd" => AvgD(values), 
			"avgi" => AvgI(values), 
			"avgdate" => AvgDate(values), 
			"avgtime" => AvgTime(values), 
			"count" => Count(values), 
			"distinctcount" => DistinctCount(values), 
			"first" => First(values), 
			"last" => Last(values), 
			"max" => Max(values), 
			"maxd" => MaxD(values), 
			"maxi" => MaxI(values), 
			"maxdate" => MaxDate(values), 
			"maxtime" => MaxTime(values), 
			"maxstr" => MaxStr(values), 
			"median" => Median(values), 
			"mediand" => MedianD(values), 
			"mediani" => MedianI(values), 
			"min" => Min(values), 
			"mind" => MinD(values), 
			"mini" => MinI(values), 
			"mindate" => MinDate(values), 
			"mintime" => MinTime(values), 
			"minstr" => MinStr(values), 
			"mode" => Mode(values), 
			"moded" => ModeD(values), 
			"modei" => ModeI(values), 
			"sum" => Sum(values), 
			"sumd" => SumD(values), 
			"sumi" => SumI(values), 
			"sumtime" => SumTime(values), 
			"sumdistinct" => SumDistinct(values), 
			_ => throw new StiFunctionNotFoundException(function), 
		};
	}

	public static string Insert(string str, int startIndex, string subStr)
	{
		if (str == null)
		{
			return null;
		}
		if (startIndex < 0 || startIndex > str.Length || subStr == null)
		{
			return str;
		}
		return str.Insert(startIndex, subStr);
	}

	public static object InsertObject(object value, int startIndex, string subStr)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Insert(v, startIndex, subStr);
		}
		return Insert(StiValueHelper.TryToString(value), startIndex, subStr);
	}

	public static bool IsDataEqual(IStiAppDataSource dataSource, string dataColumnName, string searchColumnName)
	{
		if (ToDataName(dataColumnName) == ToDataName(searchColumnName))
		{
			return true;
		}
		if (!searchColumnName.Contains("."))
		{
			return false;
		}
		return ToDataName(dataSource.GetName() + "." + dataColumnName) == searchColumnName;
	}

	public static string Left(string str, int length = -1)
	{
		if (str == null)
		{
			return null;
		}
		if (length <= 0)
		{
			return "";
		}
		if (length >= str.Length)
		{
			return str;
		}
		return str.Substring(0, length);
	}

	public static object LeftObject(object value, int length = -1)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Left(v, length);
		}
		return Left(StiValueHelper.TryToString(value), length);
	}

	public static int Length(string str)
	{
		return str?.Length ?? 0;
	}

	public static object LengthObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToStringList(value).Select(Length).Cast<object>();
		}
		return Length(StiValueHelper.TryToString(value));
	}

	public static string Remove(string str, int startIndex, int count)
	{
		if (str == null)
		{
			return null;
		}
		if (count <= 0 || startIndex < 0 || startIndex >= str.Length)
		{
			return str;
		}
		if (startIndex + count > str.Length)
		{
			count = str.Length - startIndex;
		}
		return str.Remove(startIndex, count);
	}

	public static object RemoveObject(object value, int startIndex, int count)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Remove(v, startIndex, count);
		}
		return Remove(StiValueHelper.TryToString(value), startIndex, count);
	}

	public static string Replace(string str, string oldValue, string newValue)
	{
		if (str == null)
		{
			return null;
		}
		if (oldValue == null)
		{
			return str;
		}
		if (newValue == null)
		{
			newValue = "";
		}
		return str.Replace(oldValue, newValue);
	}

	public static object ReplaceObject(object value, string oldValue, string newValue)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Replace(v, oldValue, newValue);
		}
		return Replace(StiValueHelper.TryToString(value), oldValue, newValue);
	}

	public static string Right(string str, int length = -1)
	{
		if (str == null)
		{
			return null;
		}
		if (length <= 0)
		{
			return "";
		}
		if (length >= str.Length)
		{
			return str;
		}
		return str.Substring(str.Length - length, length);
	}

	public static object RightObject(object value, int length = -1)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Right(v, length);
		}
		return Right(StiValueHelper.TryToString(value), length);
	}

	public static string ToDataName(string name)
	{
		if (name == null)
		{
			return null;
		}
		lock (toDataNameCache)
		{
			if (toDataNameCache.ContainsKey(name))
			{
				return toDataNameCache[name];
			}
			string text = name.ToLowerInvariant().Replace(" ", "");
			if (text.StartsWith("["))
			{
				text = text.Substring(1);
			}
			if (text.EndsWith("]"))
			{
				text = text.Substring(0, text.Length - 1);
			}
			toDataNameCache.Add(name, text);
			return text;
		}
	}

	public static string ToExpression(string name)
	{
		return ToExpression(name, null);
	}

	public static string ToExpression(string sourceName, string columnName)
	{
		if (sourceName == null)
		{
			return null;
		}
		sourceName = sourceName.Replace(" ", "");
		if (sourceName.Length == 0)
		{
			return string.Empty;
		}
		if (!string.IsNullOrEmpty(columnName))
		{
			columnName = columnName.Replace(" ", "");
			if (sourceName.Any((char c) => !char.IsLetterOrDigit(c) && c != '_') || columnName.Any((char c) => !char.IsLetterOrDigit(c) && c != '_') || char.IsDigit(sourceName[0]) || char.IsDigit(columnName[0]))
			{
				return "[" + sourceName + "." + columnName + "]";
			}
			return sourceName + "." + columnName;
		}
		if (sourceName.Any((char c) => !char.IsLetterOrDigit(c) && c != '.' && c != '_') || char.IsDigit(sourceName[0]))
		{
			return "[" + sourceName + "]";
		}
		return sourceName;
	}

	public static string ToLowerCase(string str)
	{
		if (str == null)
		{
			return null;
		}
		lock (toLowerCaseCache)
		{
			if (toLowerCaseCache.ContainsKey(str))
			{
				return toLowerCaseCache[str];
			}
			string text = str.ToLowerInvariant();
			toLowerCaseCache.Add(str, text);
			return text;
		}
	}

	public static object ToLowerCaseObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToStringList(value).Select(ToLowerCase);
		}
		return ToLowerCase(StiValueHelper.TryToString(value));
	}

	public static string ToProperCase(string str)
	{
		if (str == null)
		{
			return null;
		}
		lock (toProperCaseCache)
		{
			if (toProperCaseCache.ContainsKey(str))
			{
				return toProperCaseCache[str];
			}
			string text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(str.ToLowerInvariant());
			toProperCaseCache.Add(str, text);
			return text;
		}
	}

	public static object ToProperCaseObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToStringList(value).Select(ToProperCase);
		}
		return ToProperCase(StiValueHelper.TryToString(value));
	}

	public static string ToString(object value)
	{
		if (value != null)
		{
			return value.ToString();
		}
		return string.Empty;
	}

	public static object ToStringObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToList(value).Select(ToString);
		}
		return ToString(value);
	}

	public static string ToUpperCase(string str)
	{
		if (str == null)
		{
			return null;
		}
		lock (toUpperCaseCache)
		{
			if (toUpperCaseCache.ContainsKey(str))
			{
				return toUpperCaseCache[str];
			}
			string text = str.ToUpperInvariant();
			toUpperCaseCache.Add(str, text);
			return text;
		}
	}

	public static object ToUpperCaseObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToStringList(value).Select(ToUpperCase);
		}
		return ToUpperCase(StiValueHelper.TryToString(value));
	}

	public static string Trim(string str)
	{
		return str?.Trim();
	}

	public static object TrimObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToStringList(value).Select(Trim);
		}
		return Trim(StiValueHelper.TryToString(value));
	}

	public static string TrimStart(string str)
	{
		return str?.TrimStart(new char[0]);
	}

	public static object TrimStartObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToStringList(value).Select(TrimStart);
		}
		return TrimStart(StiValueHelper.TryToString(value));
	}

	public static string TrimEnd(string str)
	{
		return str?.TrimEnd(new char[0]);
	}

	public static object TrimEndObject(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToStringList(value).Select(TrimEnd);
		}
		return TrimEnd(StiValueHelper.TryToString(value));
	}

	public static string Substring(string str, int startIndex, int length = -1)
	{
		if (str == null)
		{
			return null;
		}
		if (startIndex < -1)
		{
			return "";
		}
		if (length == -1)
		{
			if (startIndex < str.Length)
			{
				return str.Substring(startIndex);
			}
			return "";
		}
		if (startIndex >= str.Length)
		{
			return "";
		}
		if (startIndex + length <= str.Length)
		{
			return str.Substring(startIndex, length);
		}
		return str.Substring(startIndex);
	}

	public static object SubstringObject(object value, int startIndex, int length = -1)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Substring(v, startIndex, length);
		}
		return Substring(StiValueHelper.TryToString(value), startIndex, length);
	}

	public static object GetSystemVariable(StiSystemVariableObject variable, int line)
	{
		return variable switch
		{
			StiSystemVariableObject.Line => line, 
			StiSystemVariableObject.LineABC => ToABC(line), 
			StiSystemVariableObject.LineRoman => ToRoman(line), 
			_ => throw new NotImplementedException(), 
		};
	}

	public static List<string> GetMapIdents(string key)
	{
		return StiReportAssembly.GetMapKeyHelper()?.GetMapIdents(key);
	}

	public static List<string> GetIso2ConvertedValues(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return null;
		}
		if (!iso2Cache.ContainsKey(name))
		{
			return new List<string> { name };
		}
		return iso2Cache[name];
	}

	public static string Iso2(string name, string mapId, string lang)
	{
		if (name == null)
		{
			return string.Empty;
		}
		IStiMapKeyHelper mapKeyHelper = StiReportAssembly.GetMapKeyHelper();
		if (mapKeyHelper == null)
		{
			return name;
		}
		string isoAlpha2FromName = mapKeyHelper.GetIsoAlpha2FromName(name, mapId, lang);
		string text = ((isoAlpha2FromName != null) ? isoAlpha2FromName : name);
		List<string> list = (iso2Cache.ContainsKey(text) ? iso2Cache[text] : null);
		if (list == null)
		{
			list = new List<string> { name };
			iso2Cache[text] = list;
		}
		else if (!list.Contains(name))
		{
			list.Add(name);
		}
		return text;
	}

	public static object Iso2Object(object value, string mapId, string lang)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Iso2(v, mapId, lang);
		}
		return Iso2(StiValueHelper.TryToString(value), mapId, lang);
	}

	public static string Iso2ToName(string alpha2, string mapId, string lang)
	{
		IStiMapKeyHelper mapKeyHelper = StiReportAssembly.GetMapKeyHelper();
		if (mapKeyHelper == null)
		{
			return alpha2;
		}
		string nameFromIsoAlpha = mapKeyHelper.GetNameFromIsoAlpha2(alpha2, mapId, lang);
		if (nameFromIsoAlpha == null)
		{
			return alpha2;
		}
		return nameFromIsoAlpha;
	}

	public static object Iso2ToNameObject(object value, string mapId, string lang)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Iso2ToName(v, mapId, lang);
		}
		return Iso2ToName(StiValueHelper.TryToString(value), mapId, lang);
	}

	public static string Iso3(string name, string mapId, string lang)
	{
		if (name == null)
		{
			return string.Empty;
		}
		IStiMapKeyHelper mapKeyHelper = StiReportAssembly.GetMapKeyHelper();
		if (mapKeyHelper == null)
		{
			return name;
		}
		string isoAlpha3FromName = mapKeyHelper.GetIsoAlpha3FromName(name, mapId, lang);
		if (isoAlpha3FromName == null)
		{
			return name;
		}
		return isoAlpha3FromName;
	}

	public static object Iso3Object(object value, string mapId, string lang)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Iso3(v, mapId, lang);
		}
		return Iso3(StiValueHelper.TryToString(value), mapId, lang);
	}

	public static string Iso3ToName(string alpha3, string mapId, string lang)
	{
		IStiMapKeyHelper mapKeyHelper = StiReportAssembly.GetMapKeyHelper();
		if (mapKeyHelper == null)
		{
			return alpha3;
		}
		string nameFromIsoAlpha = mapKeyHelper.GetNameFromIsoAlpha3(alpha3, mapId, lang);
		if (nameFromIsoAlpha == null)
		{
			return alpha3;
		}
		return nameFromIsoAlpha;
	}

	public static object Iso3ToNameObject(object value, string mapId, string lang)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select Iso3ToName(v, mapId, lang);
		}
		return Iso3ToName(StiValueHelper.TryToString(value), mapId, lang);
	}

	public static string NormalizeName(string alpha3, string mapId, string lang)
	{
		IStiMapKeyHelper mapKeyHelper = StiReportAssembly.GetMapKeyHelper();
		if (mapKeyHelper == null)
		{
			return alpha3;
		}
		return mapKeyHelper.NormalizeName(alpha3, mapId, lang);
	}

	public static object NormalizeNameObject(object value, string mapId, string lang)
	{
		if (ListExt.IsList(value))
		{
			return from v in ListExt.ToStringList(value)
				select NormalizeName(v, mapId, lang);
		}
		return NormalizeName(StiValueHelper.TryToString(value), mapId, lang);
	}
}
