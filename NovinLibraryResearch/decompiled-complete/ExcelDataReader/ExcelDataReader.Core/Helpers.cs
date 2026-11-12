using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ExcelDataReader.Misc;

namespace ExcelDataReader.Core;

internal static class Helpers
{
	private static readonly Regex EscapeRegex = new Regex("_x([0-9A-F]{4,4})_");

	public static bool IsSingleByteEncoding(Encoding encoding)
	{
		return encoding.GetByteCount(new char[1] { 'a' }) == 1;
	}

	public static double Int64BitsToDouble(long value)
	{
		return BitConverter.ToDouble(BitConverter.GetBytes(value), 0);
	}

	public static string ConvertEscapeChars(string input)
	{
		return EscapeRegex.Replace(input, (Match m) => ((char)uint.Parse(m.Groups[1].Value, NumberStyles.HexNumber)).ToString());
	}

	public static double AdjustOADateTime(double value, bool date1904)
	{
		if (!date1904)
		{
			if (value >= 0.0 && value < 60.0)
			{
				return value + 1.0;
			}
			return value;
		}
		return value + 1462.0;
	}

	public static bool IsValidOADateTime(double value)
	{
		if (value > -657435.0)
		{
			return value < 2958466.0;
		}
		return false;
	}

	public static object ConvertFromOATime(double value, bool date1904)
	{
		double num = AdjustOADateTime(value, date1904);
		if (IsValidOADateTime(num))
		{
			return DateTimeHelper.FromOADate(num);
		}
		return value;
	}

	public static object ConvertFromOATime(int value, bool date1904)
	{
		double num = AdjustOADateTime(value, date1904);
		if (IsValidOADateTime(num))
		{
			return DateTimeHelper.FromOADate(num);
		}
		return value;
	}
}
