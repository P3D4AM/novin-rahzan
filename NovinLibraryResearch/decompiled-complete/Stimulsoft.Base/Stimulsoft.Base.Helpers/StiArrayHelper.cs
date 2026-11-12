using System;

namespace Stimulsoft.Base.Helpers;

public static class StiArrayHelper
{
	public static double?[] ToNullableDoubleArray(object[] values)
	{
		int num = 0;
		double?[] array = new double?[values.Length];
		foreach (object value in values)
		{
			try
			{
				array[num] = (double)Convert.ToDecimal(value);
			}
			catch
			{
				array[num] = null;
			}
			num++;
		}
		return array;
	}

	public static double[] ToDoubleArray(object[] values)
	{
		int num = 0;
		double[] array = new double[values.Length];
		foreach (object obj in values)
		{
			if (obj == null || obj == DBNull.Value)
			{
				continue;
			}
			try
			{
				if (obj is string)
				{
					if (double.TryParse(obj as string, out var result))
					{
						array[num] = result;
					}
				}
				else
				{
					array[num] = (double)Convert.ToDecimal(obj);
				}
			}
			catch
			{
			}
			num++;
		}
		return array;
	}
}
