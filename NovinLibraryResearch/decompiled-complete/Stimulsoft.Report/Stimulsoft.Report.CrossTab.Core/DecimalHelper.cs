using System;

namespace Stimulsoft.Report.CrossTab.Core;

public static class DecimalHelper
{
	public static bool CanConvertToDecimal(object value)
	{
		if (value == null || DBNull.Value.Equals(value))
		{
			return false;
		}
		if (!(value is int) && !(value is uint) && !(value is long) && !(value is ulong) && !(value is sbyte) && !(value is byte) && !(value is float) && !(value is double))
		{
			return value is decimal;
		}
		return true;
	}

	public static decimal ConvertToDecimal(object value)
	{
		try
		{
			if (value is string && ((string)value == "-" || (string)value == ""))
			{
				return 0m;
			}
			return Convert.ToDecimal(value);
		}
		catch
		{
			return 0m;
		}
	}
}
