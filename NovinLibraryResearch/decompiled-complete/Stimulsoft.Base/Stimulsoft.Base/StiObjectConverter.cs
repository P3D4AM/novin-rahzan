using System;
using System.Text;
using System.Threading;

namespace Stimulsoft.Base;

public class StiObjectConverter
{
	public static decimal ConvertToDecimal(object value)
	{
		try
		{
			if (value == null)
			{
				return 0m;
			}
			if (value is string)
			{
				if (((string)value).Length == 0)
				{
					return 0m;
				}
				string s = NormalizeFloatingPointValue(value);
				if (decimal.TryParse(s, out var result))
				{
					return result;
				}
				if (double.TryParse(s, out var result2))
				{
					return (decimal)result2;
				}
			}
			return (decimal)Convert.ChangeType(value, typeof(decimal));
		}
		catch
		{
			return 0m;
		}
	}

	public static double ConvertToDouble(object value)
	{
		try
		{
			if (value == null)
			{
				return 0.0;
			}
			if (value is string)
			{
				if (string.IsNullOrWhiteSpace((string)value))
				{
					return 0.0;
				}
				string s = NormalizeFloatingPointValue(value);
				if (double.TryParse(s, out var result))
				{
					return result;
				}
			}
			return (double)Convert.ChangeType(value, typeof(double));
		}
		catch
		{
			return 0.0;
		}
	}

	public static long ConvertToInt64(object value)
	{
		try
		{
			if (value == null)
			{
				return 0L;
			}
			if (value is string && long.TryParse(value as string, out var result))
			{
				return result;
			}
			return (long)Convert.ChangeType(value, typeof(long));
		}
		catch
		{
			return 0L;
		}
	}

	public static string ConvertToString(byte[] bytes)
	{
		if (bytes == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in bytes)
		{
			stringBuilder.Append(b.ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	private static string NormalizeFloatingPointValue(object value)
	{
		return ((string)value).Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
	}
}
