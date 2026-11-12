using System;
using System.Globalization;
using System.Threading;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Base;

public static class StiConvert
{
	public static bool UseDefaultDateTimeForNullValues { get; set; }

	public static DateTime DefaultDateTimeForNullValues { get; set; } = DateTime.MinValue;


	private static bool IsNullableType(Type type)
	{
		if (type.IsGenericType)
		{
			return type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}
		return false;
	}

	public static object ChangeType(object value, Type conversionType)
	{
		return ChangeType(value, conversionType, convertNulls: true);
	}

	public static object ChangeType(object value, Type conversionType, bool convertNulls)
	{
		try
		{
			if ((value == null || value is DBNull) && convertNulls)
			{
				return ChangeTypeFromNullValue(conversionType);
			}
			if (value is string)
			{
				if (conversionType == typeof(string))
				{
					return value;
				}
				if (conversionType == typeof(decimal) || conversionType == typeof(double) || conversionType == typeof(float) || conversionType == typeof(decimal?) || conversionType == typeof(double?) || conversionType == typeof(float?))
				{
					string text = value as string;
					string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
					text = text.Replace(".", ",").Replace(",", numberDecimalSeparator);
					value = text.Trim();
					if (text.Length == 0)
					{
						value = "0";
					}
				}
				else if ((conversionType == typeof(int) || conversionType == typeof(long) || conversionType == typeof(byte) || conversionType == typeof(short) || conversionType == typeof(uint) || conversionType == typeof(ulong) || conversionType == typeof(sbyte) || conversionType == typeof(ushort) || conversionType == typeof(int?) || conversionType == typeof(long?) || conversionType == typeof(byte?) || conversionType == typeof(short?) || conversionType == typeof(uint?) || conversionType == typeof(ulong?) || conversionType == typeof(sbyte?) || conversionType == typeof(ushort?)) && ((string)value).Length == 0)
				{
					value = "0";
				}
			}
			if (conversionType.IsEnum)
			{
				if (value is int || value is uint || value is long || value is ulong || value is short || value is ushort || value is byte || value is sbyte)
				{
					return Enum.ToObject(conversionType, (int)value);
				}
				if (value != null && value.GetType().IsEnum)
				{
					return Enum.ToObject(conversionType, value);
				}
			}
			if (IsNullableType(conversionType) && value != null && Nullable.GetUnderlyingType(value.GetType()) != null)
			{
				if (value is int)
				{
					return (int)value;
				}
				if (value is uint)
				{
					return (uint)value;
				}
				if (value is long)
				{
					return (long)value;
				}
				if (value is ulong)
				{
					return (ulong)value;
				}
				if (value is byte)
				{
					return (byte)value;
				}
				if (value is sbyte)
				{
					return (sbyte)value;
				}
				if (value is short)
				{
					return (short)value;
				}
				if (value is ushort)
				{
					return (ushort)value;
				}
				if (value is bool)
				{
					return (bool)value;
				}
				if (value is char)
				{
					return (char)value;
				}
				if (value is double)
				{
					return (double)value;
				}
				if (value is float)
				{
					return (float)value;
				}
				if (value is decimal)
				{
					return (decimal)value;
				}
				if (value is DateTime)
				{
					return (DateTime)value;
				}
				if (value is TimeSpan)
				{
					return (TimeSpan)value;
				}
			}
			if (value is IConvertible)
			{
				return ChangeTypeToConvertible(value, conversionType);
			}
			return value;
		}
		catch
		{
			return value;
		}
	}

	private static object ChangeTypeToConvertible(object value, Type conversionType)
	{
		if (value is DBNull)
		{
			return null;
		}
		if (value is string && conversionType == typeof(Guid))
		{
			return Guid.Parse(value as string);
		}
		if (conversionType == typeof(DateTime) || conversionType == typeof(DateTime?))
		{
			return ChangeTypeToDateTime(value, conversionType);
		}
		if (conversionType == typeof(DateTimeOffset))
		{
			return ChangeTypeToDateTimeOffset(value, conversionType);
		}
		if (conversionType == typeof(TimeSpan))
		{
			return ChangeTypeToTimeSpan(value, conversionType);
		}
		if (conversionType.IsEnum)
		{
			int num = StiValueHelper.TryToInt(value);
			if (Enum.IsDefined(conversionType, num))
			{
				return Enum.ToObject(conversionType, num);
			}
			return null;
		}
		Type conversionType2 = Nullable.GetUnderlyingType(conversionType) ?? conversionType;
		return Convert.ChangeType(value, conversionType2);
	}

	private static object ChangeTypeToTimeSpan(object value, Type conversionType)
	{
		try
		{
			if (value is string)
			{
				if (string.IsNullOrWhiteSpace(value as string))
				{
					return null;
				}
				if (TimeSpan.TryParse(value as string, out var result))
				{
					return result;
				}
				return null;
			}
			if (conversionType.IsEnum)
			{
				return Enum.Parse(conversionType, value.ToString());
			}
			return Convert.ChangeType(value, conversionType);
		}
		catch
		{
			return null;
		}
	}

	private static object ChangeTypeToDateTimeOffset(object value, Type conversionType)
	{
		try
		{
			if (value is string)
			{
				if (string.IsNullOrWhiteSpace(value as string))
				{
					return null;
				}
				string text = value as string;
				if (DateTimeOffset.TryParse(text, out var result))
				{
					return result;
				}
				if (text.TryParseDateTime(out var dateTime))
				{
					return new DateTimeOffset(dateTime);
				}
				return null;
			}
			return Convert.ChangeType(value, conversionType);
		}
		catch
		{
			return null;
		}
	}

	private static object ChangeTypeToDateTime(object value, Type conversionType)
	{
		try
		{
			if (value is string)
			{
				if (string.IsNullOrWhiteSpace(value as string))
				{
					return null;
				}
				string value2 = value as string;
				if (value2.TryParseDateTime(out var dateTime))
				{
					return dateTime;
				}
				return null;
			}
			if (value is double)
			{
				if (DateTime.TryParse(value.ToString(), new CultureInfo("en-US", useUserOverride: true), DateTimeStyles.None, out var result))
				{
					return result;
				}
				if (DateTime.TryParse(value.ToString(), out result))
				{
					return result;
				}
			}
			if (value is DateTime)
			{
				return value;
			}
			if (value == null || value == DBNull.Value)
			{
				return null;
			}
			if (value.GetType().IsNumericType())
			{
				return null;
			}
			if (value.GetType().IsEnum)
			{
				return null;
			}
			return Convert.ChangeType(value, conversionType);
		}
		catch
		{
			return null;
		}
	}

	private static object ChangeTypeFromNullValue(Type conversionType)
	{
		if (conversionType == typeof(string))
		{
			return string.Empty;
		}
		if (conversionType == typeof(bool))
		{
			return false;
		}
		if (conversionType.IsEnum)
		{
			return Enum.ToObject(conversionType, 0);
		}
		if (conversionType == typeof(Guid?) || conversionType == typeof(Guid))
		{
			return Guid.Empty;
		}
		if (conversionType == typeof(DateTime))
		{
			if (!UseDefaultDateTimeForNullValues)
			{
				return null;
			}
			return DefaultDateTimeForNullValues;
		}
		if (conversionType == typeof(char))
		{
			return ' ';
		}
		if (!conversionType.IsValueType)
		{
			return null;
		}
		if (conversionType == typeof(int?))
		{
			return null;
		}
		if (conversionType == typeof(uint?))
		{
			return null;
		}
		if (conversionType == typeof(long?))
		{
			return null;
		}
		if (conversionType == typeof(ulong?))
		{
			return null;
		}
		if (conversionType == typeof(byte?))
		{
			return null;
		}
		if (conversionType == typeof(sbyte?))
		{
			return null;
		}
		if (conversionType == typeof(short?))
		{
			return null;
		}
		if (conversionType == typeof(ushort?))
		{
			return null;
		}
		if (conversionType == typeof(bool?))
		{
			return null;
		}
		if (conversionType == typeof(char?))
		{
			return null;
		}
		if (conversionType == typeof(double?))
		{
			return null;
		}
		if (conversionType == typeof(float?))
		{
			return null;
		}
		if (conversionType == typeof(decimal?))
		{
			return null;
		}
		if (conversionType == typeof(TimeSpan?))
		{
			return null;
		}
		try
		{
			if (conversionType == typeof(DateTime?))
			{
				return UseDefaultDateTimeForNullValues ? ((object)DefaultDateTimeForNullValues) : null;
			}
			return Convert.ChangeType(0, conversionType);
		}
		catch
		{
			return string.Empty;
		}
	}
}
