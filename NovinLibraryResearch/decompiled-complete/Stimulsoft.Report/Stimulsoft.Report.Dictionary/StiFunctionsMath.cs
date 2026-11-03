using System;

namespace Stimulsoft.Report.Dictionary;

public class StiFunctionsMath
{
	public static decimal Maximum(decimal value1, decimal value2)
	{
		return Math.Max(value1, value2);
	}

	public static decimal? Maximum(decimal? value1, decimal? value2)
	{
		if (value1.HasValue && value2.HasValue)
		{
			return Math.Max(value1.Value, value2.Value);
		}
		return value1 ?? value2 ?? null;
	}

	public static double Maximum(double value1, double value2)
	{
		return Math.Max(value1, value2);
	}

	public static double? Maximum(double? value1, double? value2)
	{
		if (value1.HasValue && value2.HasValue)
		{
			return Math.Max(value1.Value, value2.Value);
		}
		return value1 ?? value2 ?? null;
	}

	public static long Maximum(long value1, long value2)
	{
		return Math.Max(value1, value2);
	}

	public static long? Maximum(long? value1, long? value2)
	{
		if (value1.HasValue && value2.HasValue)
		{
			return Math.Max(value1.Value, value2.Value);
		}
		return value1 ?? value2 ?? null;
	}

	public static decimal Minimum(decimal value1, decimal value2)
	{
		return Math.Min(value1, value2);
	}

	public static decimal? Minimum(decimal? value1, decimal? value2)
	{
		if (value1.HasValue && value2.HasValue)
		{
			return Math.Min(value1.Value, value2.Value);
		}
		return value1 ?? value2 ?? null;
	}

	public static double Minimum(double value1, double value2)
	{
		return Math.Min(value1, value2);
	}

	public static double? Minimum(double? value1, double? value2)
	{
		if (value1.HasValue && value2.HasValue)
		{
			return Math.Min(value1.Value, value2.Value);
		}
		return value1 ?? value2 ?? null;
	}

	public static long Minimum(long value1, long value2)
	{
		return Math.Min(value1, value2);
	}

	public static long? Minimum(long? value1, long? value2)
	{
		if (value1.HasValue && value2.HasValue)
		{
			return Math.Min(value1.Value, value2.Value);
		}
		return value1 ?? value2 ?? null;
	}

	public static long Div(long value1, long value2)
	{
		return Div(value1, value2, 0L);
	}

	public static long Div(long value1, long value2, long zeroResult)
	{
		if (value2 == 0L)
		{
			return zeroResult;
		}
		return value1 / value2;
	}

	public static double Div(double value1, double value2)
	{
		return Div(value1, value2, 0.0);
	}

	public static double Div(double value1, double value2, double zeroResult)
	{
		if (value2 == 0.0)
		{
			return zeroResult;
		}
		return value1 / value2;
	}

	public static decimal Div(decimal value1, decimal value2)
	{
		return Div(value1, value2, 0m);
	}

	public static decimal Div(decimal value1, decimal value2, decimal zeroResult)
	{
		if (value2 == 0m)
		{
			return zeroResult;
		}
		return value1 / value2;
	}

	public static long? Div(long? value1, long? value2)
	{
		return Div(value1, value2, 0L);
	}

	public static long? Div(long? value1, long? value2, long? zeroResult)
	{
		if (!value1.HasValue || !value2.HasValue)
		{
			return zeroResult ?? null;
		}
		if (value2 == 0)
		{
			return zeroResult ?? null;
		}
		return value1 / value2;
	}

	public static double? Div(double? value1, double? value2)
	{
		return Div(value1, value2, 0.0);
	}

	public static double? Div(double? value1, double? value2, double? zeroResult)
	{
		if (!value1.HasValue || !value2.HasValue)
		{
			return zeroResult ?? null;
		}
		if (value2 == 0.0)
		{
			return zeroResult ?? null;
		}
		return value1 / value2;
	}

	public static decimal? Div(decimal? value1, decimal? value2)
	{
		return Div(value1, value2, default(decimal));
	}

	public static decimal? Div(decimal? value1, decimal? value2, decimal? zeroResult)
	{
		if (!value1.HasValue || !value2.HasValue)
		{
			return zeroResult ?? null;
		}
		decimal? num = value2;
		if ((num.GetValueOrDefault() == default(decimal)) & num.HasValue)
		{
			return zeroResult ?? null;
		}
		return value1 / value2;
	}

	public static double Round(double value)
	{
		return Math.Round(value, StiOptions.Engine.MidpointRounding);
	}

	public static decimal Round(decimal value)
	{
		return Math.Round(value, StiOptions.Engine.MidpointRounding);
	}

	public static double Round(double value, int decimals)
	{
		return Math.Round(value, decimals, StiOptions.Engine.MidpointRounding);
	}

	public static decimal Round(decimal value, int decimals)
	{
		return Math.Round(value, decimals, StiOptions.Engine.MidpointRounding);
	}

	public static double Round(double value, MidpointRounding midpointRounding)
	{
		return Math.Round(value, midpointRounding);
	}

	public static decimal Round(decimal value, MidpointRounding midpointRounding)
	{
		return Math.Round(value, midpointRounding);
	}

	public static double Round(double value, int decimals, MidpointRounding midpointRounding)
	{
		return Math.Round(value, decimals, midpointRounding);
	}

	public static decimal Round(decimal value, int decimals, MidpointRounding midpointRounding)
	{
		return Math.Round(value, decimals, midpointRounding);
	}

	public static double? Round(double? value)
	{
		if (value.HasValue)
		{
			return Math.Round(value.Value, StiOptions.Engine.MidpointRounding);
		}
		return null;
	}

	public static decimal? Round(decimal? value)
	{
		if (value.HasValue)
		{
			return Math.Round(value.Value, StiOptions.Engine.MidpointRounding);
		}
		return null;
	}

	public static double? Round(double? value, int decimals)
	{
		if (value.HasValue)
		{
			return Math.Round(value.Value, decimals, StiOptions.Engine.MidpointRounding);
		}
		return null;
	}

	public static decimal? Round(decimal? value, int decimals)
	{
		if (value.HasValue)
		{
			return Math.Round(value.Value, decimals, StiOptions.Engine.MidpointRounding);
		}
		return null;
	}

	public static double? Round(double? value, MidpointRounding midpointRounding)
	{
		if (value.HasValue)
		{
			return Math.Round(value.Value, midpointRounding);
		}
		return null;
	}

	public static decimal? Round(decimal? value, MidpointRounding midpointRounding)
	{
		if (value.HasValue)
		{
			return Math.Round(value.Value, midpointRounding);
		}
		return null;
	}

	public static double? Round(double? value, int decimals, MidpointRounding midpointRounding)
	{
		if (value.HasValue)
		{
			return Math.Round(value.Value, decimals, midpointRounding);
		}
		return null;
	}

	public static decimal? Round(decimal? value, int decimals, MidpointRounding midpointRounding)
	{
		if (value.HasValue)
		{
			return Math.Round(value.Value, decimals, midpointRounding);
		}
		return null;
	}
}
