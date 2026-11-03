using System;

namespace Stimulsoft.Report.Gauge.Helpers;

internal static class StiMathHelper
{
	public static float Length(float value1, float value2)
	{
		if (value1 < 0f && value2 < 0f)
		{
			return Math.Abs(value1 - value2);
		}
		if (value1 > 0f && value2 > 0f)
		{
			return value2 - value1;
		}
		return Math.Abs(value1) + value2;
	}

	public static float MaxMinusMin(float value1, float value2)
	{
		if (!(value1 > value2))
		{
			return value2 - value1;
		}
		return value1 - value2;
	}

	public static float GetMax(params float[] list)
	{
		float num = 0f;
		if (list.Length != 0)
		{
			num = list[0];
		}
		for (int i = 1; i < list.Length; i++)
		{
			if (num < list[i])
			{
				num = list[i];
			}
		}
		return num;
	}
}
