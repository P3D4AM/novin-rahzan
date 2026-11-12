using System;
using System.Text;
using System.Threading;

namespace Stimulsoft.Base.Helpers;

public static class StiValueHelper
{
	public static bool IsZero(object value)
	{
		if (value == null)
		{
			return true;
		}
		return Type.GetTypeCode(value.GetType()) switch
		{
			TypeCode.SByte => (sbyte)value == 0, 
			TypeCode.Int16 => (short)value == 0, 
			TypeCode.UInt16 => (ushort)value == 0, 
			TypeCode.Int32 => (int)value == 0, 
			TypeCode.UInt32 => (uint)value == 0, 
			TypeCode.Int64 => (long)value == 0, 
			TypeCode.UInt64 => (ulong)value == 0, 
			TypeCode.Single => (float)value == 0f, 
			TypeCode.Double => (double)value == 0.0, 
			TypeCode.Decimal => (decimal)value == 0m, 
			_ => false, 
		};
	}

	public static bool EqualDecimal(object value1, object value2)
	{
		if (value1 == null || value2 == null)
		{
			return false;
		}
		return TryToDecimal(value1) == TryToDecimal(value2);
	}

	public static bool ContainsNumeric(object value)
	{
		if (value == null)
		{
			return false;
		}
		if (value.GetType().IsNumericType())
		{
			return true;
		}
		if (value is string)
		{
			string s = NormalizeFloatingPointValue(value);
			double result;
			return double.TryParse(s, out result);
		}
		return false;
	}

	public static char TryToChar(object value)
	{
		if (value == null)
		{
			return ' ';
		}
		string text = value.ToString();
		if (text.Length <= 0)
		{
			return ' ';
		}
		return text[0];
	}

	public static string TryToString(object value)
	{
		if (value is string)
		{
			return value as string;
		}
		return value?.ToString();
	}

	public static float TryToFloat(object value)
	{
		try
		{
			if (value == null || value == DBNull.Value)
			{
				return 0f;
			}
			if (value is bool)
			{
				return ((bool)value) ? 1 : 0;
			}
			if (!(value is float result))
			{
				if (value is string)
				{
					string s = NormalizeFloatingPointValue(value);
					float.TryParse(s, out var result2);
					return result2;
				}
				if (!value.GetType().IsNumericType())
				{
					return 0f;
				}
				return Convert.ToSingle(value);
			}
			return result;
		}
		catch
		{
			return 0f;
		}
	}

	public static double TryToDouble(object value)
	{
		try
		{
			if (value == null || value == DBNull.Value)
			{
				return 0.0;
			}
			if (value is bool)
			{
				return ((bool)value) ? 1 : 0;
			}
			if (!(value is double result))
			{
				if (value is string)
				{
					string s = NormalizeFloatingPointValue(value);
					double.TryParse(s, out var result2);
					return result2;
				}
				if (!value.GetType().IsNumericType())
				{
					return 0.0;
				}
				return Convert.ToDouble(value);
			}
			return result;
		}
		catch
		{
			return 0.0;
		}
	}

	public static decimal TryToDecimal(object value)
	{
		try
		{
			if (value == null || value == DBNull.Value)
			{
				return 0m;
			}
			if (value is bool)
			{
				return ((bool)value) ? 1 : 0;
			}
			if (!(value is decimal result))
			{
				if (value is string)
				{
					string s = NormalizeFloatingPointValue(value);
					if (decimal.TryParse(s, out var result2))
					{
						return result2;
					}
					if (double.TryParse(s, out var result3))
					{
						return (decimal)result3;
					}
					return 0m;
				}
				if (!value.GetType().IsNumericType())
				{
					return 0m;
				}
				return Convert.ToDecimal(value);
			}
			return result;
		}
		catch
		{
			return 0m;
		}
	}

	public static bool TryToBool(object value)
	{
		try
		{
			if (value == null || value == DBNull.Value)
			{
				return false;
			}
			if (!(value is bool result))
			{
				decimal? num = TryToNullableDecimal(value);
				if (num.HasValue)
				{
					return num.Value == 1m;
				}
				if (value is string text)
				{
					string text2 = text.ToLowerInvariant();
					return text2 == "true" || text2 == "on" || text2 == "yes";
				}
				return false;
			}
			return result;
		}
		catch
		{
			return false;
		}
	}

	public static int TryToInt(object value)
	{
		try
		{
			if (value == null || value == DBNull.Value)
			{
				return 0;
			}
			if (value is bool)
			{
				return ((bool)value) ? 1 : 0;
			}
			if (!(value is int result))
			{
				if (value is string)
				{
					int.TryParse((string)value, out var result2);
					return result2;
				}
				if (!value.GetType().IsNumericType())
				{
					return 0;
				}
				return Convert.ToInt16(value);
			}
			return result;
		}
		catch
		{
			return 0;
		}
	}

	internal static uint TryToUInt(object value)
	{
		try
		{
			if (value == null || value == DBNull.Value)
			{
				return 0u;
			}
			if (value is bool)
			{
				return ((bool)value) ? 1u : 0u;
			}
			if (!(value is uint result))
			{
				if (value is string)
				{
					uint.TryParse((string)value, out var result2);
					return result2;
				}
				if (!value.GetType().IsNumericType())
				{
					return 0u;
				}
				return Convert.ToUInt16(value);
			}
			return result;
		}
		catch
		{
			return 0u;
		}
	}

	public static long TryToLong(object value)
	{
		try
		{
			if (value == null || value == DBNull.Value)
			{
				return 0L;
			}
			if (value is bool)
			{
				return ((bool)value) ? 1 : 0;
			}
			if (!(value is long result))
			{
				if (value is string)
				{
					long.TryParse((string)value, out var result2);
					return result2;
				}
				if (!value.GetType().IsNumericType())
				{
					return 0L;
				}
				return Convert.ToInt64(value);
			}
			return result;
		}
		catch
		{
			return 0L;
		}
	}

	internal static ulong TryToULong(object value)
	{
		try
		{
			if (value == null || value == DBNull.Value)
			{
				return 0uL;
			}
			if (value is bool)
			{
				return (ulong)(int)(((bool)value) ? 1u : 0u);
			}
			if (!(value is ulong result))
			{
				if (value is string)
				{
					ulong.TryParse((string)value, out var result2);
					return result2;
				}
				if (!value.GetType().IsNumericType())
				{
					return 0uL;
				}
				return Convert.ToUInt64(value);
			}
			return result;
		}
		catch
		{
			return 0uL;
		}
	}

	public static DateTime TryToDateTime(object value)
	{
		try
		{
			if (!(value is DateTime result))
			{
				if (value == null || value == DBNull.Value)
				{
					return DateTime.Now;
				}
				if (value is string && string.IsNullOrWhiteSpace(value as string))
				{
					return DateTime.Now;
				}
				if (value.GetType().IsNumericType())
				{
					return DateTime.Now;
				}
				if (value.GetType().IsEnum)
				{
					return DateTime.Now;
				}
				return (DateTime)StiConvert.ChangeType(value, typeof(DateTime), convertNulls: false);
			}
			return result;
		}
		catch
		{
			return DateTime.Now;
		}
	}

	public static TimeSpan TryToTimeSpan(object value)
	{
		try
		{
			if (!(value is TimeSpan result))
			{
				if (value == null || value == DBNull.Value)
				{
					return new TimeSpan(0L);
				}
				return (TimeSpan)StiConvert.ChangeType(value, typeof(TimeSpan), convertNulls: false);
			}
			return result;
		}
		catch
		{
			return new TimeSpan(0L);
		}
	}

	public static float? TryToNullableFloat(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return null;
		}
		try
		{
			if (value is string)
			{
				string s = NormalizeFloatingPointValue(value);
				if (float.TryParse(s, out var result))
				{
					return result;
				}
				return null;
			}
			if (value.GetType().IsEnum)
			{
				return (int)value;
			}
			if (!value.GetType().IsNumericType())
			{
				return 0f;
			}
			return Convert.ToSingle(value);
		}
		catch
		{
			return null;
		}
	}

	public static double? TryToNullableDouble(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return null;
		}
		try
		{
			if (value is DateTime dateTime)
			{
				return dateTime.ToOADate();
			}
			if (value is string)
			{
				string s = NormalizeFloatingPointValue(value);
				if (double.TryParse(s, out var result))
				{
					return result;
				}
				return null;
			}
			if (value.GetType().IsEnum)
			{
				return (int)value;
			}
			if (!value.GetType().IsNumericType())
			{
				return 0.0;
			}
			return Convert.ToDouble(value);
		}
		catch
		{
			return null;
		}
	}

	public static decimal? TryToNullableDecimal(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return null;
		}
		try
		{
			if (value is string)
			{
				string s = NormalizeFloatingPointValue(value);
				if (decimal.TryParse(s, out var result))
				{
					return result;
				}
				return null;
			}
			if (value.GetType().IsEnum)
			{
				return (int)value;
			}
			if (!value.GetType().IsNumericType())
			{
				return null;
			}
			return Convert.ToDecimal(value);
		}
		catch
		{
			return null;
		}
	}

	public static long? TryToNullableLong(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return null;
		}
		try
		{
			if (value is string)
			{
				if (long.TryParse((string)value, out var result))
				{
					return result;
				}
				return null;
			}
			if (value.GetType().IsEnum)
			{
				return (int)value;
			}
			if (!value.GetType().IsNumericType())
			{
				return null;
			}
			return Convert.ToInt64(value);
		}
		catch
		{
			return null;
		}
	}

	public static int? TryToNullableInt(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return null;
		}
		try
		{
			if (value is string)
			{
				if (int.TryParse((string)value, out var result))
				{
					return result;
				}
				return null;
			}
			if (value.GetType().IsEnum)
			{
				return (int)value;
			}
			if (!value.GetType().IsNumericType())
			{
				return null;
			}
			return Convert.ToInt16(value);
		}
		catch
		{
			return null;
		}
	}

	internal static ulong? TryToNullableULong(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return null;
		}
		try
		{
			if (value is string)
			{
				if (ulong.TryParse((string)value, out var result))
				{
					return result;
				}
				return null;
			}
			if (value.GetType().IsEnum)
			{
				return (ulong)(int)value;
			}
			if (!value.GetType().IsNumericType())
			{
				return null;
			}
			return Convert.ToUInt64(value);
		}
		catch
		{
			return null;
		}
	}

	internal static uint? TryToNullableUInt(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return null;
		}
		try
		{
			if (value is string)
			{
				if (uint.TryParse((string)value, out var result))
				{
					return result;
				}
				return null;
			}
			if (value.GetType().IsEnum)
			{
				return (uint)(int)value;
			}
			if (!value.GetType().IsNumericType())
			{
				return null;
			}
			return Convert.ToUInt16(value);
		}
		catch
		{
			return null;
		}
	}

	public static DateTime? TryToNullableDateTime(object value)
	{
		if (value is DateTime)
		{
			return (DateTime)value;
		}
		if (value == null)
		{
			return null;
		}
		try
		{
			return (DateTime?)StiConvert.ChangeType(value, typeof(DateTime?), convertNulls: false);
		}
		catch
		{
			return null;
		}
	}

	public static TimeSpan? TryToNullableTimeSpan(object value)
	{
		if (value is TimeSpan)
		{
			return (TimeSpan)value;
		}
		if (value == null)
		{
			return null;
		}
		try
		{
			return (TimeSpan?)StiConvert.ChangeType(value, typeof(TimeSpan?), convertNulls: false);
		}
		catch
		{
			return null;
		}
	}

	public static Guid TryToGuid(object value)
	{
		if (value is Guid)
		{
			return (Guid)value;
		}
		if (value == null)
		{
			return Guid.Empty;
		}
		try
		{
			return (Guid)StiConvert.ChangeType(value, typeof(Guid), convertNulls: false);
		}
		catch
		{
			return Guid.Empty;
		}
	}

	public static decimal ParseDecimal(string value)
	{
		if (value == "0" || string.IsNullOrWhiteSpace(value))
		{
			return 0m;
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 1;
		bool flag = true;
		foreach (char c in value)
		{
			if (c == ',' || c == '.')
			{
				flag = false;
				continue;
			}
			if (flag)
			{
				stringBuilder = stringBuilder.Append(c);
				continue;
			}
			stringBuilder2 = stringBuilder2.Append(c);
			num *= 10;
		}
		if (stringBuilder.Length == 0 && stringBuilder2.Length != 0)
		{
			return (decimal)long.Parse(stringBuilder2.ToString()) / (decimal)num;
		}
		if (stringBuilder.Length != 0 && stringBuilder2.Length != 0)
		{
			return (decimal)long.Parse(stringBuilder.ToString()) + (decimal)long.Parse(stringBuilder2.ToString()) / (decimal)num;
		}
		if (stringBuilder.Length != 0 && stringBuilder2.Length == 0)
		{
			return long.Parse(stringBuilder.ToString());
		}
		return 0m;
	}

	public static double ParseDouble(string value)
	{
		if (value == "0" || string.IsNullOrWhiteSpace(value))
		{
			return 0.0;
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 1;
		bool flag = true;
		foreach (char c in value)
		{
			if (c == ',' || c == '.')
			{
				flag = false;
				continue;
			}
			if (flag)
			{
				stringBuilder = stringBuilder.Append(c);
				continue;
			}
			stringBuilder2 = stringBuilder2.Append(c);
			num *= 10;
		}
		if (stringBuilder.Length == 0 && stringBuilder2.Length != 0)
		{
			return (double)long.Parse(stringBuilder2.ToString()) / (double)num;
		}
		if (stringBuilder.Length != 0 && stringBuilder2.Length != 0)
		{
			return (double)long.Parse(stringBuilder.ToString()) + (double)long.Parse(stringBuilder2.ToString()) / (double)num;
		}
		if (stringBuilder.Length != 0 && stringBuilder2.Length == 0)
		{
			return long.Parse(stringBuilder.ToString());
		}
		return 0.0;
	}

	public static float ParseFloat(string value)
	{
		if (value == "0" || string.IsNullOrWhiteSpace(value))
		{
			return 0f;
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 1;
		bool flag = true;
		foreach (char c in value)
		{
			if (c == ',' || c == '.')
			{
				flag = false;
				continue;
			}
			if (flag)
			{
				stringBuilder = stringBuilder.Append(c);
				continue;
			}
			stringBuilder2 = stringBuilder2.Append(c);
			num *= 10;
		}
		if (stringBuilder.Length == 0 && stringBuilder2.Length != 0)
		{
			return (float)long.Parse(stringBuilder2.ToString()) / (float)num;
		}
		if (stringBuilder.Length != 0 && stringBuilder2.Length != 0)
		{
			return (float)long.Parse(stringBuilder.ToString()) + (float)long.Parse(stringBuilder2.ToString()) / (float)num;
		}
		if (stringBuilder.Length != 0 && stringBuilder2.Length == 0)
		{
			return long.Parse(stringBuilder.ToString());
		}
		return 0f;
	}

	private static string NormalizeFloatingPointValue(object value)
	{
		return ((string)value).Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
	}
}
