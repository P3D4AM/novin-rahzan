using System;

namespace Stimulsoft.Report.Chart;

internal static class StiStripLineCalculatorXF
{
	private static double GetInterval(double interval)
	{
		double num = Math.Abs(interval);
		int num2 = 0;
		if (interval > 1.0)
		{
			while (num > 1.0)
			{
				num /= 10.0;
				num2++;
			}
			num = ((num < 0.15) ? 0.1 : ((num < 0.3) ? 0.2 : ((!(num < 0.75)) ? 1.0 : 0.5)));
			while (num2 > 0)
			{
				num *= 10.0;
				num2--;
			}
		}
		else if (interval > 0.0)
		{
			while (num * 10.0 < 1.0)
			{
				num *= 10.0;
				num2++;
			}
			num = ((num < 0.15) ? 0.1 : ((num < 0.3) ? 0.2 : ((!(num < 0.75)) ? 1.0 : 0.5)));
			while (num2 > 0)
			{
				num /= 10.0;
				num2--;
			}
		}
		return num;
	}

	internal static double GetInterval(double minValue, double maxValue, int num)
	{
		if (maxValue == minValue)
		{
			return 0.0;
		}
		return GetInterval((maxValue - minValue) / (double)num);
	}

	internal static StiStripLinesXF GetStripLines(double minValue, double maxValue, double step, bool asDateTimeValue, bool defaultMinValue = false)
	{
		StiStripLinesXF stiStripLinesXF = new StiStripLinesXF();
		if (minValue == maxValue)
		{
			return stiStripLinesXF;
		}
		decimal num = (decimal)minValue;
		decimal num2 = (decimal)maxValue;
		decimal num3 = (decimal)step;
		decimal num4;
		for (num4 = (defaultMinValue ? num : ((decimal)(long)(num / num3) * num3)); num < num4 + num3; num4 -= num3)
		{
		}
		while (num4 < num2)
		{
			num4 += num3;
			if (asDateTimeValue)
			{
				stiStripLinesXF.Insert(0, new StiStripLineXF(DateTime.FromOADate((double)num4), (double)num4));
			}
			else
			{
				stiStripLinesXF.Insert(0, new StiStripLineXF(num4.ToString(), (double)num4));
			}
		}
		return stiStripLinesXF;
	}

	internal static StiStripLinesXF GetStripLinesLogScale(double minValue, double maxValue)
	{
		StiStripLinesXF stiStripLinesXF = new StiStripLinesXF();
		if (minValue == maxValue)
		{
			return stiStripLinesXF;
		}
		decimal num = (decimal)minValue;
		decimal num2 = (decimal)maxValue;
		decimal num3 = 1m;
		decimal num4 = 1m;
		if (0.0 < minValue && minValue < 1.0)
		{
			for (; num < num3; num3 /= 10m)
			{
			}
		}
		if (minValue > 1.0)
		{
			for (; num > num3; num3 *= 10m)
			{
			}
			num3 /= 10m;
		}
		if (0m < num2 && num2 < 1m)
		{
			for (; num4 > num2; num4 /= 10m)
			{
			}
			num4 *= 10m;
		}
		if (num2 > 1m)
		{
			for (; num2 > num4; num4 *= 10m)
			{
			}
		}
		decimal num5 = num3;
		decimal num6 = num5;
		int num7 = 1;
		while (num5 <= num4)
		{
			stiStripLinesXF.Insert(0, new StiStripLineXF(((double)num5).ToString(), (double)num5));
			num5 += num6;
			num7++;
			if (num7 == 10)
			{
				num7 = 1;
				num6 = num5;
			}
		}
		return stiStripLinesXF;
	}
}
